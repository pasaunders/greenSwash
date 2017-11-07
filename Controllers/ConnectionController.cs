using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using greenSwash.Models;

namespace greenSwash.Controllers
{
    public class ConnectionController : Controller
    {
        private greenSwashContext _context;
        public ConnectionController(greenSwashContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            if (HttpContext.Session.GetInt32("currentUserId") == null)
            {
                return RedirectToAction("Index", "User");
            }
            int loggedInUserId = (int)HttpContext.Session.GetInt32("currentUserId");
            Users loggedInUser = _context.users
                .Include(item => item.connector).ThenInclude(item => item.connector)
                .Include(item => item.connector).ThenInclude(item => item.connected)
                .Include(item => item.connected).ThenInclude(item => item.connector)
                .Include(item => item.connected).ThenInclude(item => item.connected)
                .FirstOrDefault(item => item.usersId ==loggedInUserId);
            IEnumerable<Connections> acceptedOne = loggedInUser.connected.Where(item => item.confirmed == 1);
            IEnumerable<Connections> acceptedTwo = loggedInUser.connector.Where(item => item.confirmed == 0);
            List<Connections> networkConnections = acceptedOne.Concat(acceptedTwo).ToList();
            List<Users> networkPeopleOne = networkConnections
                .Where(item => item.connector != loggedInUser)
                .Select(item => item.connector)
                .ToList();
            List<Users> networkPeopleTwo = networkConnections
                .Where(item => item.connected != loggedInUser)
                .Select(item => item.connected)
                .ToList();
            profileViewModel profileData = new profileViewModel
            {
                name = loggedInUser.name,
                description = loggedInUser.description,
                network = networkPeopleOne.Concat(networkPeopleTwo).ToList(),
                invitations = loggedInUser.connected.Where(item => item.confirmed == 0).ToList()
            };
            return View(profileData);
        }
        [HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            Users chosenUser = _context.users
                .FirstOrDefault(item => item.usersId == id);
            if (chosenUser != null)
            {
                ViewBag.thisUser = chosenUser;
                return View();
            }
            return RedirectToAction("list");
        }
        [HttpGet]
        [Route("accept/{id}")]
        public IActionResult Accept(int id)
        {
            Connections chosenConnection = _context.connections
                .FirstOrDefault(item => item.connectionId == id);
            chosenConnection.confirmed = 1;
            _context.SaveChanges();
            return RedirectToAction("list");
        }
        [HttpGet]
        [Route("ignore/{id}")]
        public IActionResult Ignore(int id)
        {
            int loggedInUserId = (int)HttpContext.Session.GetInt32("currentUserId");
            Connections chosenConnection = _context.connections
                .FirstOrDefault(item => item.connectionId == id);
            Users currentUser = _context.users
                .Include(item => item.connector)
                .Include(item => item.connected)
                .FirstOrDefault(item => item.usersId ==loggedInUserId);
            currentUser.connected.Remove(chosenConnection);
            _context.SaveChanges();
            return RedirectToAction("list");
        }
        [HttpGet]
        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            int loggedInUserId = (int)HttpContext.Session.GetInt32("currentUserId");
            Users currentUser = _context.users
                .Include(item => item.connector)
                .Include(item => item.connected)
                .FirstOrDefault(item => item.usersId ==loggedInUserId);
            Connections chosenConnection = _context.connections
                .FirstOrDefault(item => (item.connectedId == id && item.connectorId == loggedInUserId) || (item.connectedId == loggedInUserId && item.connectorId == id));
            currentUser.connected.Remove(chosenConnection);
            currentUser.connector.Remove(chosenConnection);
            _context.connections.Remove(chosenConnection);
            _context.SaveChanges();
            return RedirectToAction("list");
        }
        [HttpGet]
        [Route("users")]
        public IActionResult Connect(int id)
        {
            int loggedInUserId = (int)HttpContext.Session.GetInt32("currentUserId");
            Users currentUser = _context.users
                .Include(item => item.connector).ThenInclude(item => item.connector)
                .Include(item => item.connector).ThenInclude(item => item.connected)
                .Include(item => item.connected).ThenInclude(item => item.connector)
                .Include(item => item.connected).ThenInclude(item => item.connected)
                .FirstOrDefault(item => item.usersId == loggedInUserId);
            List<Users> newPeople = _context.users
                .Include(item => item.connector)
                .Include(item => item.connected)
                .Where(item => item.connected
                    .Any(connected => connected.connectedId != currentUser.usersId && connected.connectorId != currentUser.usersId)
                )
                .Where(item => item.connector
                    .Any(connector => connector.connectedId != currentUser.usersId && connector.connectorId != currentUser.usersId)
                )
                .ToList();
            // ViewBag.newPeople = newPeople;
            ViewBag.newPeople = _context.users.ToList();
            ViewBag.currentUser = currentUser;
            return View("AllUsers");
        }
        [HttpGet]
        [Route("makeconnection/{id}")]
        public IActionResult MakeConnection(int id)
        {
            int loggedInUserId = (int)HttpContext.Session.GetInt32("currentUserId");
            Users currentUser = _context.users.FirstOrDefault(item => item.usersId == loggedInUserId);
            Connections newConnection = new Connections();
            newConnection.connectorId = currentUser.usersId;
            newConnection.connector = currentUser;
            newConnection.connectedId = id;
            newConnection.connected = _context.users.FirstOrDefault(item => item.usersId == id);
            newConnection.confirmed = 0;
            _context.connections.Add(newConnection);
            _context.SaveChanges();
            currentUser.connector.Add(newConnection);
            newConnection.connected.connected.Add(newConnection);
            _context.SaveChanges();
            return RedirectToAction("Connect");
        }
    }
}