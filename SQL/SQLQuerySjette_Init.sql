-- Author: Thibaut Deliever
-- TestQuery for inserting (randomized) data in the database.
USE TheGreatFinChallenge
GO

--First Command
--Initialize data for table dbo.Users in database 'TheGreatFinChallenge'.
--15 Unique users with own email and same password ("@Dummie123").
--Password: gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=
--Salt: XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=
INSERT INTO Users (FirstName, LastName, [Admin], Email,	PasswordHash, [Hash], CreationDate)
VALUES 
	('Thibaut',	'Deliever',		1, 'thibaut.deliever@gmail.com',	'gqcDhyRRIRr3FEm1QMiToj28k4W65cy3X/nsHLarE1M=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22');


--Second Command
--Initialize data for table dbo.Groups in database 'TheGreatFinChallenge'.
--5 Unique groups.
INSERT INTO Groups (fk_CreatorID, GroupName)
VALUES	
	(1, 'Ranking'),
	(1, 'Directie'),
	(1, 'Informaticacel & Douanecel'),
	(1, 'Geschillen'),
	(1, 'Invordering'),
	(1, 'Input'),
	(1, 'Inspectie Gent 1'),
	(1, 'Inspectie Gent 2'),
	(1, 'Inspectie Brugge');