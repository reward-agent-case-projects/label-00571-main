using Dqsm.Backend.Data;
using Dqsm.Backend.Models;
using Dqsm.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dqsm.Backend.Controllers
{
    public class WeChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dashboard for messages
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _context.UserMsgs
                .OrderByDescending(m => m.DateTime)
                .Take(50)
                .ToListAsync();
            return View(messages);
        }

        /// <summary>
        /// Receive user interaction (Simulated Endpoint)
        /// Requirement: 接收微信公众号用户的互动信息
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] UserMsgDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var msg = new UserMsg
            {
                FromUser = input.FromUser,
                ToUser = input.ToUser,
                Message = input.Message,
                DateTime = DateTime.Now
            };

            _context.UserMsgs.Add(msg);
            await _context.SaveChangesAsync();

            // Log is handled by Filter automatically, but we can add specific debug info
            StaticLogService.Debug("WeChat/ReceiveMessage", $"Message received from {input.FromUser}");

            return Ok(new { success = true, msgId = msg.Id });
        }

        /// <summary>
        /// Send Template Message
        /// Requirement: 发送模板信息
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SendTemplate([FromBody] TemplateMsgDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Mock sending to WeChat API
            bool sendSuccess = true; // Assume success

            if (sendSuccess)
            {
                var template = new TemplateMsg
                {
                    FromUser = "SYSTEM", // Or the official account ID
                    ToUser = input.ToUser,
                    Message = input.TemplateDataJson,
                    DateTime = DateTime.Now
                };

                _context.TemplateMsgs.Add(template);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, templateId = template.Id });
            }

            return StatusCode(500, "Failed to send to WeChat");
        }
    }

    // DTOs
    public class UserMsgDto
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
    }

    public class TemplateMsgDto
    {
        public string ToUser { get; set; }
        public string TemplateDataJson { get; set; }
    }
}
