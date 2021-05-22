using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheGreatFinChallenge.Models;
using TheGreatFinChallenge.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data;

namespace TheGreatFinChallenge.Controllers
{
    [Authorize] // Make sure that only authorized users can use methods of this controller. If not, the user gets redirected to the login with a returnurl to the requested page.
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly TGFCContext _context;
        private IHostingEnvironment _environment;
        private Dictionary<string, string> UserDictionairy = new Dictionary<string, string>();

        public AccountController(ILogger<AccountController> logger, TGFCContext context, IHostingEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }


        /* 
         * Dictionairy that is used inside this controller to pass userinformation between functions.
         * This is an alternative for the User.Claims. Because of the internal use inside this controller,
         * the functions is set to private. 
        */
        private void setUserDictionairy()
        {
            foreach (var c in User.Claims)
            {
                UserDictionairy[c.Type.ToString()] = c.Value.ToString();
            }
        }


        private bool HasSpecialChars(string testString)
        {
            return testString.Any(c => !Char.IsLetterOrDigit(c));
        }


        private bool HasDigit(string testString) {
            return testString.Any(char.IsDigit);
        }


        /*
         * Function to hash a password with a given hash and non-hashed version of the password.
         * This function is private because of the internal use inside this controller.
        */
        private string hashPassword(string hash, string password)
        {
            byte[] salt = Convert.FromBase64String(hash);

            string returnPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return returnPassword;
        } 

        
        /*
         * Function that returns an userObject of the SQL DB with a given context and email.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Users> getUserByEmailAsync(TGFCContext ctx, string email)
        {
            var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                               $"FROM Users AS U " +
                                               $"WHERE U.Email = '{email}'").ToListAsync();
            if (x.Count == 0) return null;
            return x.First();
        }


        private async Task<List<Users>> getAllUsersByIdAsync(TGFCContext ctx)
        {
            var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                               $"FROM Users AS U ").ToListAsync();
            return x;
        }


        private async Task<List<GroupMembership>> getAllGroupMembershipsAsync(TGFCContext ctx)
        {
            var x = await ctx.GroupMembership.FromSqlRaw($"SELECT GM.* " +
                                                         $"FROM GroupMembership AS GM ").ToListAsync();
            return x;
        }

        private async Task<List<Activities>> getAllActivitiesAsync(TGFCContext ctx)
        {
            var x = await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                    $"FROM Activities AS A ").ToListAsync();
            return x;
        }

        /*
         * Function that returns an userObject of the SQL DB with a given context and id.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Users> getUserByIdAsync(TGFCContext ctx, int id)
        {
            return await ctx.Users.FindAsync(id);
        }


        private static async Task<Photos> getPhotoByIdAsync(TGFCContext ctx, int id)
        {
            return await ctx.Photos.FindAsync(id);
        }


        /*
         * Function that returns an activityObject of the SQL DB with a given context and id.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Activities> getActivityByIdAsync(TGFCContext ctx, int id)
        {
            return await ctx.Activities.FindAsync(id);
        }


        /*
         * Function that returns an groupObject of the SQL DB with a given context and groupName.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Groups> getGroupByNameAsync(TGFCContext ctx, string groupName)
        {
            var x = await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                                $"FROM Groups AS G " +
                                                $"WHERE G.GroupName = '{groupName}'").ToListAsync();
            return x.First();
        }


        /* 
         * Function that returns a List of groupObjects of the SQL DB with a given context and userObject.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<Groups>> getGroupsOfUserAsync(TGFCContext ctx, Users user)
        {
            return await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                               $"FROM Groups AS G " +
                                               $"INNER JOIN GroupMembership AS GM ON G.pk_GroupID = GM.GroupID " +
                                               $"INNER JOIN Users AS U ON GM.UserId = U.pk_UserID " +
                                               $"WHERE U.pk_UserID={user.pk_UserID}").ToListAsync();
        }

        private static async Task<List<Groups>> getAllGroupsAsync(TGFCContext ctx)
        {
            return await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                               $"FROM Groups AS G ").ToListAsync();
        }


        /* 
         * Function that returns a List of activityObjects of the SQL DB with a given context and userObject 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<Activities>> getActivitiesOfUserAsync(TGFCContext ctx, Users user)
        {
            return await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                   $"FROM Activities AS A " +
                                                   $"WHERE a.fk_UserID={user.pk_UserID} " +
                                                   $"AND MONTH(A.StartTime) = 6 " + 
                                                   $"ORDER BY StartTime DESC").ToListAsync();
        }


        /* 
         * Function that returns a Dictionary with as key the primaryKey of a group and as value all the activities
         * of all users of a group, with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Dictionary<int, List<Activities>>> getActivitiesOfGroupAsync(TGFCContext ctx, List<Groups> groupsOfUser)
        {
            Dictionary<int, List <Activities>> ReturnDict = new Dictionary<int, List<Activities>>();
            foreach (Groups group in groupsOfUser)
            {
                var x = await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                        $"FROM Activities AS A " +
                                                        $"WHERE a.fk_UserID in " +
                                                            $"(SELECT G.UserID " +
                                                            $"FROM GroupMembership AS G " +
                                                            $"WHERE G.GroupID = {group.pk_GroupID}) " +
                                                        $"AND MONTH(A.StartTime) = 6 " +
                                                        $"ORDER BY StartTime DESC").ToListAsync();
                ReturnDict[group.pk_GroupID] = x;
            }
            return ReturnDict;
        }


        /* 
         * Function that returns a Dictionary with as key the primaryKey of a group and as value all the user
         * of a group, with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Dictionary<int, List<Users>>> getAllUsersOfGroupAsync(TGFCContext ctx, List<Groups> groupsOfUser)
        {
            Dictionary<int, List<Users>> ReturnDict = new Dictionary<int, List<Users>>();
            foreach (Groups group in groupsOfUser)
            {
                var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                                   $"FROM Users AS U " +
                                                   $"WHERE U.pk_UserID IN " +
                                                       $"(SELECT G.UserID " +
                                                       $" FROM GroupMembership AS G " +
                                                       $" WHERE G.GroupID = {group.pk_GroupID})").ToListAsync();
                ReturnDict[group.pk_GroupID] = x;
            }
            return ReturnDict;
        }
        private static async Task<List<Users>> getAllUsersOfGroupAsync(TGFCContext ctx, Groups group)
        {
            var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                               $"FROM Users AS U " +
                                               $"WHERE U.pk_UserID IN " +
                                                   $"(SELECT G.UserID " +
                                                   $" FROM GroupMembership AS G " +
                                                   $" WHERE G.GroupID = {group.pk_GroupID})").ToListAsync();
            return x;
        }


        /* 
         * Function that returns a list of mutual users with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<MutualUsers>> getMutualUsers(TGFCContext ctx)
        {
            var x = await ctx.MutualUsers.FromSqlRaw($"SELECT ROW_NUMBER() OVER(ORDER BY GroupID ASC) AS Row, " +
                                                     $"U.*, GM.GroupID " + 
                                                     $"FROM Users AS U " +
                                                     $"INNER JOIN GroupMembership AS GM " +
                                                     $"ON U.pk_UserID = GM.UserID").ToListAsync();
            return x;
        }


        /* 
         * Function that returns a list of groupMembershipObjects with a given context and an userId. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<GroupMembership>> GetGroupMembershipsOfUserAsync(TGFCContext ctx, int userId)
        {
            var x = await ctx.GroupMembership.FromSqlRaw($"SELECT GM.* " +
                                                         $"FROM GroupMembership AS GM " +
                                                         $"WHERE GM.UserID={userId}").ToListAsync();
            return x;
        }


        private static async Task<List<Photos>> getAllPhotos(TGFCContext ctx)
        {
            var x = await ctx.Photos.FromSqlRaw($"SELECT P.* " +
                                                $"FROM Photos AS P ").ToListAsync();
            return x;
        }



        // GET: Account/
        public async Task<IActionResult> Index()
        {
            setUserDictionairy();

            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var groups = await getGroupsOfUserAsync(_context, user);
            var allGroups = await getAllGroupsAsync(_context);
            var activities = await getActivitiesOfUserAsync(_context, user);
            var groupActivities = await getActivitiesOfGroupAsync(_context, groups);

            DashboardData data = new DashboardData(user, groups, allGroups, activities, groupActivities);

            return View(data);
        }


        // GET: Account/Activity/
        public async Task<IActionResult> Activity()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var activities = await getActivitiesOfUserAsync(_context, user);

            ActivityData data = new ActivityData(user,activities);

            return View(data);
        }


        // GET: Account/createActivity
        public IActionResult createActivity()
        {
            return View();
        }


        [HttpPost("createActivity")]
        public async Task<IActionResult> createNewActivity(string activityType, string totalKms, DateTime startTime, TimeSpan totalTime, int calories)
        {
            if (totalKms.Contains(".")) totalKms = totalKms.Replace(".", ",");
            setUserDictionairy();

            Activities Activity = new Activities();
            Activity.fk_UserID = Convert.ToInt32(UserDictionairy["UserID"]);
            Activity.ActivityType = activityType;
            Activity.Distance = Convert.ToDecimal(totalKms);
            Activity.StartTime = startTime;
            Activity.TTime = totalTime;
            Activity.TotalCalories = calories;

            _context.Add(Activity);
            await _context.SaveChangesAsync();
            TempData["Succes"] = "The activity has been succesfully created.";

            return Redirect("~/Account");
        }

        // POST: /editActivity
        [HttpPost("editActivity")]
        public async Task<IActionResult> editActivity(string redirectUrl, string activityId, DateTime Date, TimeSpan Duration, string Distance, int Calories, string Gear)
        {
            if (Distance.Contains(".")) Distance = Distance.Replace(".", ",");
            setUserDictionairy();

            Activities Activity = await getActivityByIdAsync(_context, Convert.ToInt32(activityId));
            Activity.Distance = Convert.ToDecimal(Distance);
            Activity.StartTime = Date;
            Activity.TTime = Duration;
            Activity.Gear = Gear;
            Activity.TotalCalories = Calories;

            _context.Activities.Update(Activity);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "The activity-data has been succesfully updated.";
            var url = $"{redirectUrl}";
            return Redirect(url);
        }


        // POST: /deleteActivity
        [HttpPost("deleteActivity")]
        public async Task<IActionResult> deleteActivity(string redirectUrl, string activityId)
        {
            setUserDictionairy();
            Activities Activity = await getActivityByIdAsync(_context, Convert.ToInt32(activityId));

            _context.Activities.Remove(Activity);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "The activity has been succesfully deleted.";
            var url = $"{redirectUrl}";
            return Redirect(url);
        }


        // GET: Account/Group
        public async Task<IActionResult> Group()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var groups = await getGroupsOfUserAsync(_context, user);
            var allGroups = await getAllGroupsAsync(_context);
            var activities = await getActivitiesOfUserAsync(_context, user);
            var groupActivities = await getActivitiesOfGroupAsync(_context, allGroups);
            var allUsersOfGroup = await getAllUsersOfGroupAsync(_context, allGroups);
            var allUsersById = await getAllUsersByIdAsync(_context);

            GroupData data = new GroupData(user, groups, allGroups, activities, groupActivities, allUsersOfGroup, allUsersById);

            return View(data);
        }


        // GET: Account/Settings/
        public async Task<IActionResult> Settings()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            return View(user);
        }

        // POST: /saveSettings
        [HttpPost("saveSettings")]
        public async Task<IActionResult> SaveSettings(string firstName, string lastName, string email, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            bool done = false;
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var oldPasswordHash = (oldPassword != null) ? hashPassword(user.Hash, oldPassword):"";

            if (firstName != null)
            {
                user.FirstName = firstName;
                done = true;
            }
            if (lastName != null)
            {
                user.LastName = lastName;
                done = true;
            }
            if (email != null)
            {
                user.Email = email;
                done = true;
            }

            if (user.PasswordHash == oldPasswordHash && newPassword == newPasswordConfirm && newPassword != null)
            {
                if (HasSpecialChars(newPassword) && newPassword.Length >= 7 && HasDigit(newPassword))
                {
                    user.PasswordHash = hashPassword(user.Hash, newPassword);
                    done = true;
                }
                else TempData["PasswordRequirementsError"] = "Please make sure that the new password meets the requirements.";
            }
            else if (newPassword == "")
            {
                TempData["NewPasswordError"] = "Please fill in a password";
            }
            else if (newPassword != newPasswordConfirm)
            {
                TempData["NewPasswordError"] = "Password did not match with each other.";
            } else if (user.PasswordHash != oldPasswordHash && oldPasswordHash != "")
            {
                TempData["PasswordError"] = "Old Password did not match with this account.";
            }

            if (done && TempData["PasswordRequirementsError"] == null && TempData["NewPasswordError"] == null && TempData["PasswordError"] == null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                TempData["Succes"] = "Settings were succesfully saved";
                
            }

            if (done && TempData["PasswordError"] == null && TempData["NewPasswordError"] == null && TempData["PasswordRequirementsError"] == null)
            {
                return Redirect("~/Account");
            } 
            else return Redirect("~/Account/Settings");
        }



        // GET: /logout		
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }


        // GET: /photos
        public async Task<IActionResult> Photos()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var photoList = await getAllPhotos(_context);
            var userList = await getAllUsersByIdAsync(_context);
            var user = await getUserByIdAsync(_context, id);
            var photoData = new PhotosData(user, photoList, userList);
            return View(photoData);
        }

        // POST: /Photos
        [HttpPost("Photos")]
        public async Task<ActionResult> UploadFiles(IFormFile file)
        {
            if (file != null)
            {
                string webRootPath = _environment.WebRootPath;
                string path = Path.Combine(webRootPath, "img\\uploadedImages", Path.GetFileName(file.FileName));

                setUserDictionairy();
                var id = Convert.ToInt32(UserDictionairy["UserID"]);

                Photos photo = new Photos();
                photo.PhotoName = file.FileName;
                photo.CreatorID = id;
                photo.PhotoLocation = $"../img/";
                _context.Add(photo);
                await _context.SaveChangesAsync();

                var photoId = photo.PhotoID;
                var newFileName = $"../img/uploadedImages/{photoId}{Path.GetExtension(path)}";
                path = Path.Combine(webRootPath, "img\\uploadedImages", $"{photoId}{Path.GetExtension(path)}");
                photo.PhotoLocation = newFileName;
                _context.Update(photo);
                
                try
                {
                    using (Stream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    TempData["Succes"] = "The file has been succesfully uploaded.";
                } catch
                {
                    TempData["Error"] = $"Something went wrong while uploading.";
                    _context.Remove(photo);
                }
            } else
            {
                TempData["Error"] = "Choose a valid file please.";
            }

            await _context.SaveChangesAsync();
            return Redirect("~/Account/Photos");
        }


        // POST: /deleteActivity
        [HttpPost("DeletePhotos")]
        public async Task<ActionResult> DeleteFile(int objId)
        {
            Photos photo = await getPhotoByIdAsync(_context, objId);

            string webRootPath = _environment.WebRootPath;
            var extention = Path.GetExtension(photo.PhotoLocation);
            var fileName = $"{Convert.ToString(photo.PhotoID)}{extention}";
            string path = Path.Combine(webRootPath, "img\\uploadedImages", fileName);


            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                _context.Remove(photo);
                await _context.SaveChangesAsync();
                TempData["Succes"] = "The file has been succesfully deleted.";
            }
            else
            {
                TempData["Error"] = "Something went wrong while deleting this file.";
            }

            return Redirect("~/Account/Photos");
        }


        // POST: /Backup
        [HttpPost("Backup")]
        public async Task<ActionResult> GetBackupFile()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);

            if (user.Admin)
            {
                string webRootPath = _environment.WebRootPath;
                string date = DateTime.Now.ToString("dd_MM_yyyy");
                string fileName = String.Format("Backup_{0}.txt", date);
                string path = Path.Combine(webRootPath, "BackupTXT\\", fileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }


                var data = "USE TheGreatFinChallenge\nGO\n\n";

                //Users
                data += "INSERT INTO Users (FirstName, LastName, [Admin], Email, PasswordHash, [Hash], CreationDate)\nVALUES\n";
                List<Users> users = await getAllUsersByIdAsync(_context);
                foreach (var u in users)
                {
                    var line = $"('{u.FirstName}', '{u.LastName}', {Convert.ToInt32(u.Admin)},'{u.Email}', '{u.PasswordHash}', '{u.Hash}', '{u.CreationDate}')";
                    if (u == users.Last()) line += ";\n\n";
                    else line += ",\n";
                    data += line;
                }


                //Groups
                data += "INSERT INTO Groups (fk_CreatorID, GroupName)\nVALUES\n";
                List<Groups> groups = await getAllGroupsAsync(_context);
                foreach (var g in groups)
                {
                    var line = $"({g.fk_CreatorID}, '{g.GroupName}')";
                    if (g == groups.Last()) line += ";\n\n";
                    else line += ",\n";
                    data += line;
                }


                //GroupMemberships
                data += "INSERT INTO GroupMembership(UserID, GroupID)\nVALUES\n";
                List<GroupMembership> groupmemberships = await getAllGroupMembershipsAsync(_context);
                foreach (var gm in groupmemberships)
                {
                    var line = $"({gm.UserID}, {gm.GroupID})";
                    if (gm == groupmemberships.Last()) line += ";\n\n";
                    else line += ",\n";
                    data += line;
                }


                //Activities
                data += "INSERT INTO Activities (fk_UserID, ActivityType, TotalCalories, Distance, TTime, StartTime, Gear)\nVALUES\n";
                List<Activities> activities = await getAllActivitiesAsync(_context);
                foreach (var a in activities)
                {
                    var line = $"({a.fk_UserID}, '{a.ActivityType}', {a.TotalCalories}, {a.Distance}, {a.TTime}, {a.StartTime}, '{a.Gear}')";
                    if (a == activities.Last()) line += ";";
                    else line += ",\n";
                    data += line;
                }

                using (StreamWriter outputFile = new StreamWriter(path))
                {
                    await outputFile.WriteAsync(data);
                }

                return PhysicalFile(path, "application/octet-stream", fileName);
            } 
            else
            {
                TempData["Error"] = "You don't have the permissions to do this. Please contact an administrator.";
                return Redirect("~/Account/Settings");
            }
            
        }
    }
}
