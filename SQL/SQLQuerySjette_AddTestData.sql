-- Author: Thibaut Deliever
-- TestQuery for inserting (randomized) data in the database.
USE TheGreatFinChallenge
GO

--First Command
--Initialize data for table dbo.Users in database 'TheGreatFinChallenge'.
--15 Unique users with own email and same password ("@Dummie123").
--Dummies generated with 'https://nl.fakenamegenerator.com/gen-male-nl-bg.php'.
INSERT INTO Users (FirstName, LastName, [Admin], Email,	PasswordHash, [Hash], CreationDate)
VALUES 
	('Sophie',	'Test',	1, 'sophie.test@dummie.be',	'gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-30'),
	('Thibaut',	'Deliever',		1, 'thibaut.deliever@dummie.be',	'gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22'),
	('Persoon3','Dummie',		0, 'persoon3@dummie.be',			'gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-30'),
	('newUser',	'newUser',		0, 'newUser@dummie.be',				'gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-30');




--Second Command
--Initialize data for table dbo.Groups in database 'TheGreatFinChallenge'.
--5 Unique groups.
INSERT INTO Groups (fk_CreatorID, GroupName)
VALUES	
	(2, 'Ranking'),
	(2, 'Directie'),
	(2, 'Informaticacel & Douanecel'),
	(2, 'Geschillen'),
	(2, 'Invordering'),
	(2, 'Input'),
	(2, 'Inspectie Gent 1'),
	(2, 'Inspectie Gent 2'),
	(2, 'Inspectie Brugge');




--Third Command
--Initialize data for table dbo.GroupMembership in database 'TheGreatFinChallenge'.
INSERT INTO GroupMembership(UserID, GroupID)
VALUES 
	(1, 3),
	(2, 4),
	(3, 3);

--Fourth Command
--Initialize data for table dbo.Activities in database 'TheGreatFinChallenge'.
--Data generated with python file in the folder 'SQL'.
INSERT INTO Activities (fk_UserID, ActivityType, TotalCalories, Distance, TTime, StartTime, Gear)
VALUES 
(1, 'Swimming', 2872, 27.54, '06:44:13', '2021-06-19 14:00:00', 'NULL'),
(3, 'Cycling', 1289, 47.32, '01:27:41', '2021-06-10 12:00:00', 'Polar Watch'),
(1, 'Swimming', 1501, 11.4, '03:31:18', '2021-06-24 12:00:00', 'NULL'),
(1, 'Cycling', 3788, 85.96, '04:17:44', '2021-06-11 13:00:00', 'Asics Shoes'),
(3, 'Swimming', 1705, 19.54, '04:00:05', '2021-06-07 11:00:00', 'AppleWatch'),
(1, 'Swimming', 2134, 23.88, '05:00:26', '2021-06-19 15:00:00', 'Asics Shoes'),
(3, 'Hiking', 3200, 29.3, '08:42:35', '2021-06-25 15:00:00', 'AppleWatch'),
(3, 'Cycling', 2004, 50.6, '02:16:22', '2021-06-09 10:00:00', 'Asics Shoes'),
(1, 'Running', 1389, 18.25, '01:53:25', '2021-06-08 15:00:00', 'NULL'),
(2, 'Swimming', 1075, 5.17, '02:31:22', '2021-06-21 10:00:00', 'AppleWatch'),
(2, 'Running', 164, 1.47, '00:13:23', '2021-06-26 10:00:00', 'Polar Watch'),
(1, 'Swimming', 838, 7.91, '01:58:01', '2021-06-07 13:00:00', 'NULL'),
(3, 'Swimming', 489, 4.12, '01:08:55', '2021-06-04 13:00:00', 'NULL'),
(1, 'Cycling', 966, 32.04, '01:05:43', '2021-06-18 14:00:00', 'Polar Watch'),
(2, 'Running', 3237, 38.9, '04:24:14', '2021-06-20 14:00:00', 'AppleWatch'),
(2, 'Running', 2608, 35.21, '03:32:54', '2021-06-28 13:00:00', 'Nike Shoes'),
(1, 'Running', 3966, 51.91, '05:23:47', '2021-06-11 14:00:00', 'NULL'),
(1, 'Cycling', 7719, 165.34, '08:45:08', '2021-06-05 12:00:00', 'AppleWatch'),
(1, 'Cycling', 2780, 119.1, '03:09:10', '2021-06-04 10:00:00', 'NULL'),
(1, 'Swimming', 965, 8.24, '02:15:57', '2021-06-08 11:00:00', 'Nike Shoes'),
(2, 'Swimming', 2541, 23.88, '05:57:40', '2021-06-22 11:00:00', 'AppleWatch'),
(3, 'Swimming', 1377, 7.98, '03:13:51', '2021-06-13 10:00:00', 'Polar Watch'),
(1, 'Cycling', 575, 19.45, '00:39:10', '2021-06-14 12:00:00', 'NULL'),
(1, 'Running', 1194, 16.21, '01:37:30', '2021-06-14 11:00:00', 'NULL'),
(1, 'Hiking', 8190, 97.14, '22:17:15', '2021-06-04 14:00:00', 'NULL'),
(3, 'Hiking', 3288, 41.41, '08:56:57', '2021-06-09 10:00:00', 'Nike Shoes'),
(1, 'Running', 5099, 57.89, '06:56:16', '2021-06-13 15:00:00', 'Polar Watch'),
(3, 'Running', 1771, 32.77, '02:24:37', '2021-06-05 14:00:00', 'Asics Shoes'),
(2, 'Swimming', 1203, 13.35, '02:49:22', '2021-06-14 14:00:00', 'AppleWatch'),
(1, 'Cycling', 2005, 39.28, '02:16:25', '2021-06-24 15:00:00', 'Nike Shoes'),
(2, 'Swimming', 3045, 21.56, '07:08:37', '2021-06-18 13:00:00', 'NULL'),
(1, 'Swimming', 118, 1.26, '00:16:39', '2021-06-20 11:00:00', 'Asics Shoes'),
(2, 'Running', 4669, 54.03, '06:21:10', '2021-06-27 13:00:00', 'Asics Shoes'),
(3, 'Running', 2875, 25.6, '03:54:45', '2021-06-15 11:00:00', 'AppleWatch'),
(3, 'Hiking', 5061, 56.8, '13:46:21', '2021-06-15 15:00:00', 'AppleWatch'),
(2, 'Running', 1471, 23.31, '02:00:07', '2021-06-08 11:00:00', 'NULL'),
(2, 'Running', 2831, 56.51, '03:51:06', '2021-06-14 15:00:00', 'NULL'),
(3, 'Cycling', 5139, 78.27, '05:49:38', '2021-06-19 13:00:00', 'NULL'),
(3, 'Running', 5455, 49.23, '07:25:19', '2021-06-15 11:00:00', 'Nike Shoes'),
(1, 'Swimming', 579, 5.52, '01:21:29', '2021-06-02 13:00:00', 'NULL'),
(3, 'Swimming', 2651, 22.73, '06:13:13', '2021-06-15 11:00:00', 'Asics Shoes'),
(3, 'Hiking', 6249, 84.11, '17:00:22', '2021-06-13 15:00:00', 'Asics Shoes'),
(2, 'Cycling', 8918, 142.54, '10:06:41', '2021-06-14 12:00:00', 'Nike Shoes'),
(2, 'Running', 194, 2.1, '00:15:51', '2021-06-07 10:00:00', 'AppleWatch'),
(1, 'Cycling', 3607, 75.92, '04:05:26', '2021-06-27 10:00:00', 'Polar Watch'),
(2, 'Running', 2089, 22.09, '02:50:32', '2021-06-22 11:00:00', 'Asics Shoes'),
(1, 'Swimming', 294, 3.32, '00:41:27', '2021-06-03 13:00:00', 'Nike Shoes'),
(3, 'Running', 3352, 59.59, '04:33:40', '2021-06-26 11:00:00', 'Polar Watch'),
(1, 'Running', 4916, 59.15, '06:41:22', '2021-06-03 13:00:00', 'Asics Shoes'),
(2, 'Cycling', 287, 6.84, '00:19:33', '2021-06-05 14:00:00', 'Asics Shoes'),
(1, 'Cycling', 7270, 157.53, '08:14:34', '2021-06-05 11:00:00', 'Polar Watch'),
(2, 'Cycling', 3701, 132.44, '04:11:48', '2021-06-13 15:00:00', 'NULL'),
(1, 'Hiking', 2890, 32.89, '07:51:55', '2021-06-19 13:00:00', 'Polar Watch'),
(1, 'Swimming', 2319, 12.08, '05:26:25', '2021-06-01 13:00:00', 'NULL'),
(3, 'Swimming', 2542, 26.48, '05:57:53', '2021-06-02 11:00:00', 'Asics Shoes'),
(3, 'Swimming', 1728, 19.83, '04:03:20', '2021-06-28 14:00:00', 'Asics Shoes'),
(2, 'Cycling', 3893, 160.75, '04:24:52', '2021-06-09 15:00:00', 'NULL'),
(2, 'Swimming', 573, 6.63, '01:20:43', '2021-06-13 13:00:00', 'Nike Shoes'),
(2, 'Swimming', 45, 0.46, '00:06:20', '2021-06-18 13:00:00', 'NULL'),
(1, 'Cycling', 4687, 94.05, '05:18:52', '2021-06-23 10:00:00', 'Nike Shoes');
