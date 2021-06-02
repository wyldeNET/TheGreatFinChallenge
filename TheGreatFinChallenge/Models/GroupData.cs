using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatFinChallenge.Models
{
    // DashboardData model to pass multiple models in 1 combined one.
    // Also used to combine all razor code and variables.
    // Not the most efficient but cleanest way to code.
    public class GroupData
    {
        // ObjectProperties
        public Users User { get; set; }
        public List<Groups> UserGroups { get; set; }
        public List<Groups> AllGroups { get; set; }
        public Dictionary<int, List<Users>> AllUsersOfGroup { get; set; }
        public List<Users> allUsers { get; set; }
        public Dictionary<string, List<Users>> AllUsersOfGroupByGroupName { get; set; }
        public List<Activities> UserActivities { get; set; }
        public Dictionary<int, List<Activities>> ActivitiesOfAllGroups { get; set; }


        // LineCharts
        public Dictionary<string, Dictionary<string, List<int>>> LineChartData { get; set; }
        public Dictionary<string, Dictionary<string, List<int>> > DataGroupRanking { get; set; }


        // Table
        public Dictionary<string, List<string>> TableHeaders {get; set;}                                         //Activity -  Headers
        public Dictionary<int, Dictionary<string, Dictionary<string, List<int>> >> TableData { get; set; }       //GroupId  -  Dict<Activity - Dict<Username - List<numbers>>>
        public Dictionary<int, List< Tuple<string, Activities>> > TableDataRecent { get; set; }


        // Constructor
        public GroupData(Users user, List<Groups> groups, List<Groups> allGroups, List<Activities> activities, 
            Dictionary<int, List<Activities>> groupActivities, 
            Dictionary<int, List<Users>>  allUsersOfGroup, List<Users> allUsers)
        {
            string[] array1 = { "Activities", "Hiking", "Cycling", "Running", "Swimming", "Calories", "Distance"};
            List<string> listOfLineChartProperties = new List<string>(array1);

            this.User = user;
            this.UserActivities = activities;
            this.UserGroups = groups;
            this.AllGroups = allGroups;
            this.ActivitiesOfAllGroups = groupActivities;
            this.AllUsersOfGroup = allUsersOfGroup;
            this.allUsers = allUsers;

            setAllUsersOfGroupByGroupName();

            SetLineChartData(listOfLineChartProperties);
            SetDataGroupRanking(listOfLineChartProperties);

            SetTableHeaders();
            SetTableData();
            //System.Diagnostics.Debugger.Break();
        }


        private void setAllUsersOfGroupByGroupName()
        {
            var dictBeforeReturn = new Dictionary<string, List<Users>>();
            foreach (var item in AllUsersOfGroup)
            {
                int groupId = item.Key;
                var group = AllGroups.Find(x => x.pk_GroupID == groupId);
                var userList = item.Value;
                dictBeforeReturn[group.GroupName] = userList;
            }
            this.AllUsersOfGroupByGroupName = dictBeforeReturn;
        }


        


        // Set data of an user for the lineChart
        private void SetLineChartData(List<string> properties)
        {
            var tempDictBeforeReturn = new Dictionary<string, Dictionary<string, List<int>>>();
            foreach (var group in AllGroups)
            {
                var tempDictionary = new Dictionary<string, List<int>>();
                foreach (var property in properties)
                {
                    var lst = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    foreach (var activity in ActivitiesOfAllGroups[group.pk_GroupID]) 
                    {

                        var day = activity.StartTime.Day - 1;
                        var prev = lst[day];
                        if (property == "Activities" || activity.ActivityType == property) lst[day] = prev + 1;
                        if (property == "Calories") lst[day] = prev + activity.TotalCalories;
                        if (property == "Distance") lst[day] = prev + Convert.ToInt32(activity.Distance); 
                    }
                tempDictionary[property] = lst;
                }
            tempDictBeforeReturn[group.GroupName] = tempDictionary;
            }
            this.LineChartData = tempDictBeforeReturn;      //Group - Dict<Acti, Ints>>
        }


        private void SetDataGroupRanking(List<string> properties)
        {
            var tempDictBeforeReturn = new Dictionary<string, Dictionary<string, List<int>> >();       //Acti - Dict<Group - Ints>
            foreach (var property in properties)
            {
                var dict = new Dictionary<string, List<int>>();

                foreach (var group in AllGroups)
                {
                    if (group.GroupName != "Ranking")
                    {
                        int userCount = AllUsersOfGroupByGroupName[group.GroupName].Count > 1 ? AllUsersOfGroupByGroupName[group.GroupName].Count : 1;

                        var lst = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                        foreach (var activity in ActivitiesOfAllGroups[group.pk_GroupID])
                        {
                            var day = activity.StartTime.Day - 1;
                            var prev = lst[day];
                            if (property == "Activities" || activity.ActivityType == property) lst[day] = prev + 1;
                            if (property == "Calories") lst[day] = prev + activity.TotalCalories;
                            if (property == "Distance") lst[day] = prev + Convert.ToInt32(activity.Distance);
                        }

                        if (property == "Calories")
                        {
                            var newLst = new List<int>();
                            var count = 0;
                            foreach (var i in lst)
                            {
                                count += i;
                                newLst.Add(count / userCount);
                            }
                            dict[group.GroupName] = newLst;
                        }
                        else dict[group.GroupName] = lst;
                    }
                }
                tempDictBeforeReturn[property] = dict;
            }

            this.DataGroupRanking = tempDictBeforeReturn;
        }


        private void SetTableHeaders()
        {
            string[] array1 = { "Name", "Amount", "Longest Ride", "Total Distance", "Total Calories", "Fastest 50km", "Fastest 100km"};
            string[] array2 = { "Name", "Amount", "Longest Run", "Total Distance", "Total Calories", "Fastest 5km", "Fastest 10km", "Fastest 21.1km", "Fastest 42.2km" };
            string[] array3 = { "Name", "Amount", "Longest Hike", "Total Distance", "Total Calories", "Hikes 10km+", "Hikes 25km+", "Hikes 100km+"};
            string[] array4 = { "Name", "Amount", "Longest Swim", "Total Distance", "Total Calories", "Swims 1km+", "Swims 5km+", "Swims 10km+" };
            string[] array5 = { "Name", "Type", "Date", "Duration", "Distance", "Calories" };

            var tempDictBeforeReturn = new Dictionary<string, List<string>>();
            var list1 = new List<string>(array1);
            var list2 = new List<string>(array2);
            var list3 = new List<string>(array3);
            var list4 = new List<string>(array4);
            var list5 = new List<string>(array5);

            tempDictBeforeReturn["Cycling"] = list1;
            tempDictBeforeReturn["Running"] = list2;
            tempDictBeforeReturn["Hiking"] = list3;
            tempDictBeforeReturn["Swimming"] = list4;
            tempDictBeforeReturn["Recent"] = list5;

            this.TableHeaders = tempDictBeforeReturn;
        }

        private Users getUserFromId(int id)
        {
            foreach(var u in allUsers)
            {
                if(id == u.pk_UserID) return u;
            }
            return null;
        }

        private int calculateTime(TimeSpan timeDone, decimal distanceDoneDecimal, double distanceNeeded)
        {
            double distanceDone = Decimal.ToDouble(distanceDoneDecimal);
            if (distanceDone < distanceNeeded) return 0;
            return (int)(((double)distanceNeeded / distanceDone) * (int) timeDone.TotalMinutes);
        }


        //GroupId  -  Dict<Activity - Dict<UserName - List<numbers>
        private void SetTableData()
        {
            var tempDictBeforeReturn = new Dictionary<int, Dictionary<string, Dictionary<string, List<int>>>>();
            var tempDictBeforeReturn2 = new Dictionary<int, List<Tuple<string, Activities>>>();

            foreach (var item in ActivitiesOfAllGroups)
            {
                var groupId = item.Key;
                var activities = item.Value;

                var dictActivities = new Dictionary<string, Dictionary<string, List<int>>>();
                var lstActivities = new List<Tuple<string, Activities>>();


                foreach (var activity in activities)
                {
                    // Check and make userDict
                    Dictionary<string, List<int>> userDict;
                    bool dictionaryExist = dictActivities.ContainsKey(activity.ActivityType);
                    if (dictionaryExist) userDict = dictActivities[activity.ActivityType];
                    else userDict = new Dictionary<string, List<int>>();
                    var user = getUserFromId(activity.fk_UserID);

                    //Cycling
                    if (activity.ActivityType == "Cycling" && user != null)
                    {
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            int time50km = calculateTime(activity.TTime, activity.Distance, 50.0);
                            int time100km = calculateTime(activity.TTime, activity.Distance, 100.0);
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.Distance;
                            prev[3] += activity.TotalCalories;
                            if (prev[4] < time50km) prev[4] = time50km;
                            if (prev[5] < time100km) prev[5] = time100km;
                        }
                        else
                        {
                            prev = new List<int>();
                            int time50km = calculateTime(activity.TTime, activity.Distance, 50.0);
                            int time100km = calculateTime(activity.TTime, activity.Distance, 100.0);
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.Distance);
                            prev.Add(activity.TotalCalories);
                            prev.Add(time50km);
                            prev.Add(time100km);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }
                    // Running
                    else if (activity.ActivityType == "Running" && user != null)
                    {
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            int time5km = calculateTime(activity.TTime, activity.Distance, 5.0);
                            int time10km = calculateTime(activity.TTime, activity.Distance, 10.0);
                            int time21_1km = calculateTime(activity.TTime, activity.Distance, 21.1);
                            int time42_2km = calculateTime(activity.TTime, activity.Distance, 42.2);
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.Distance;
                            prev[3] += activity.TotalCalories;
                            if (prev[4] < time5km) prev[4] = time5km;
                            if (prev[5] < time10km) prev[5] = time10km;
                            if (prev[6] < time21_1km) prev[6] = time21_1km;
                            if (prev[7] < time42_2km) prev[7] = time42_2km;
                        }
                        else
                        {
                            prev = new List<int>();
                            int time5km = calculateTime(activity.TTime, activity.Distance, 5.0);
                            int time10km = calculateTime(activity.TTime, activity.Distance, 10.0);
                            int time21_1km = calculateTime(activity.TTime, activity.Distance, 21.1);
                            int time42_2km = calculateTime(activity.TTime, activity.Distance, 42.2);
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.Distance);
                            prev.Add(activity.TotalCalories);
                            prev.Add(time5km);
                            prev.Add(time10km);
                            prev.Add(time21_1km);
                            prev.Add(time42_2km);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }

                    // Hiking
                    else if (activity.ActivityType == "Hiking" && user != null)
                    {
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.Distance;
                            prev[3] += activity.TotalCalories;
                            if ((int)activity.Distance >= 10) prev[4]++;
                            if ((int)activity.Distance >= 25) prev[5]++;
                            if ((int)activity.Distance >= 100) prev[6]++;
                        }
                        else
                        {
                            prev = new List<int>();
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.Distance);
                            prev.Add(activity.TotalCalories);
                            if ((int)activity.Distance >= 10)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.Distance >= 25)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.Distance >= 100)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }

                    //Swimming
                    else if (activity.ActivityType == "Swimming" && user != null)
                    {
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.Distance;
                            prev[3] += activity.TotalCalories;
                            if ((int)activity.Distance >= 1) prev[4]++;
                            if ((int)activity.Distance >= 5) prev[5]++;
                            if ((int)activity.Distance >= 10) prev[6]++;
                        }
                        else
                        {
                            prev = new List<int>();
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.Distance);
                            prev.Add(activity.TotalCalories);
                            if ((int)activity.Distance >= 1)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.Distance >= 5)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.Distance >= 10)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    } 

                    // RECENT
                    if(user != null)
                    {
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        var tuple = new Tuple<string, Activities>(keyUsername, activity);
                        lstActivities.Add(tuple);
                    }
                    tempDictBeforeReturn[groupId] = dictActivities;
                    tempDictBeforeReturn2[groupId] = lstActivities;
                }

                this.TableData = tempDictBeforeReturn;
                this.TableDataRecent = tempDictBeforeReturn2;
            }
        }




    }
}
