SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[UserProfileId] [uniqueidentifier] NOT NULL,
	[DisplayName] [nvarchar](60) NULL,
	[Photo] [nvarchar](max) NULL,
	[Photo96x96] [nvarchar](max) NULL,
	[ManagerId] [uniqueidentifier] NULL,
	[GivenName] [nvarchar](60) NULL,
	[Mail] [nvarchar](60) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserProfiles] ADD  CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
	[UserProfileId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_UserProfiles_DisplayName] ON [dbo].[UserProfiles]
(
	[DisplayName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScorePoints](
	[ScorePointsId] [int] IDENTITY(1,1) NOT NULL,
	[KudosSent] [int] NOT NULL,
    [KudosReceived] [int] NOT NULL,
    [LikesSent] [int] NOT NULL,
    [LikesReceived] [int] NOT NULL,
    [CommentsSent] [int] NOT NULL,
    [CommentsReceived] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ScorePoints] ADD  CONSTRAINT [PK_ScorePoints] PRIMARY KEY CLUSTERED 
(
	[ScorePointsId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUsers](
	[AdminUserId] [int] IDENTITY(1,1) NOT NULL,
	[UserProfileId] [uniqueidentifier] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AdminUsers] ADD PRIMARY KEY CLUSTERED 
(
	[AdminUserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BadgeRules](
	[BadgeRulesId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ImageName] [nvarchar](max) NULL,
	[ActionType] [nvarchar](max) NOT NULL,
	[Initial] [int] NOT NULL,
	[Final] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BadgeRules] ADD  CONSTRAINT [PK_BadgeRules] PRIMARY KEY CLUSTERED 
(
	[BadgeRulesId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecognitionsGroup](
	[RecognitionGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[BadgeName] [nvarchar](max) NULL,
	[Emoji] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[RecognitionsGroup] ADD  CONSTRAINT [PK_RecognitionsGroup] PRIMARY KEY CLUSTERED 
(
	[RecognitionGroupId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recognitions](
	[RecognitionId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[RecognitionGroupId] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Recognitions] ADD  CONSTRAINT [PK_Recognitions] PRIMARY KEY CLUSTERED 
(
	[RecognitionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kudos](
	[KudosId] [int] IDENTITY(1,1) NOT NULL,
	[FromPersonId] [uniqueidentifier] NULL,
	[RecognitionId] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Date] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Kudos] ADD  CONSTRAINT [PK_Kudos] PRIMARY KEY CLUSTERED 
(
	[KudosId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_frompersonid] ON [dbo].[Kudos]
(
	[FromPersonId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KudosLike](
	[KudosLikeId] [int] IDENTITY(1,1) NOT NULL,
	[KudosId] [int] NOT NULL,
	[PersonId] [uniqueidentifier] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KudosLike] ADD  CONSTRAINT [PK_KudosLike] PRIMARY KEY CLUSTERED 
(
	[KudosLikeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_PersonId] ON [dbo].[KudosLike]
(
	[PersonId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KudosLike_KudosId] ON [dbo].[KudosLike]
(
	[KudosId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KudosLike]  WITH CHECK ADD  CONSTRAINT [FK_KudosLike_Kudos_KudosId] FOREIGN KEY([KudosId])
REFERENCES [dbo].[Kudos] ([KudosId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[KudosLike] CHECK CONSTRAINT [FK_KudosLike_Kudos_KudosId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KudosReceiver](
	[KudosReceiverId] [int] IDENTITY(1,1) NOT NULL,
	[KudosId] [int] NOT NULL,
	[ToPersonId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KudosReceiver] ADD  CONSTRAINT [PK_KudosReceiver] PRIMARY KEY CLUSTERED 
(
	[KudosReceiverId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KudosReceiver_KudosId] ON [dbo].[KudosReceiver]
(
	[KudosId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KudosReceiver_ToPersonId] ON [dbo].[KudosReceiver]
(
	[ToPersonId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KudosReceiver]  WITH CHECK ADD  CONSTRAINT [FK_KudosReceiver_Kudos_KudosId] FOREIGN KEY([KudosId])
REFERENCES [dbo].[Kudos] ([KudosId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[KudosReceiver] CHECK CONSTRAINT [FK_KudosReceiver_Kudos_KudosId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentsId] [int] IDENTITY(1,1) NOT NULL,
	[KudosId] [int] NOT NULL,
	[FromPersonId] [uniqueidentifier] NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentsId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idx_FromPersonId] ON [dbo].[Comments]
(
	[FromPersonId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Comments_KudosId] ON [dbo].[Comments]
(
	[KudosId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Kudos_KudosId] FOREIGN KEY([KudosId])
REFERENCES [dbo].[Kudos] ([KudosId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Kudos_KudosId]
GO


INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('Primeiro reconhecimento enviado', '1kudossent.png', 'send_kudos', 1, 9)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('10 reconhecimentos enviados', '1kudossent.png', 'send_kudos', 10, 49)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('50 reconhecimentos enviados', '10kudossent.png', 'send_kudos', 50, 99)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('100 reconhecimentos enviados', '100kudossent.png', 'send_kudos', 100, null)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('10 reconhecimentos recebidos', '10kudosreceived.png', 'received_kudos', 10, 49)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('50 reconhecimentos recebidos', '50kudosreceived.png', 'received_kudos', 50, 99)
INSERT INTO BadgeRules ([Description], ImageName, ActionType, Initial, Final) VALUES ('100 reconhecimentos recebidos', '100kudosreceived.png', 'received_kudos', 100, null)


create user superkudosmsgraph from EXTERNAL PROVIDER

ALTER ROLE db_datareader ADD MEMBER superkudosmsgraph;
ALTER ROLE db_datawriter ADD MEMBER superkudosmsgraph;
ALTER ROLE db_ddladmin ADD MEMBER superkudosmsgraph;


create user superkudoskudos from EXTERNAL PROVIDER

ALTER ROLE db_datareader ADD MEMBER [superkudoskudos];
ALTER ROLE db_datawriter ADD MEMBER [superkudoskudos];
ALTER ROLE db_ddladmin ADD MEMBER [superkudoskudos];


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GETTOPCONTRIBUTORS]
	@top int = 10,
    @ManagerId uniqueidentifier = null
AS

    DECLARE @ScorePointsId INT,
        @KudosSent INT,
        @KudosReceived INT,
        @LikesSent INT,
        @LikesReceived INT,
        @CommentsSent INT,
        @CommentsReceived INT;

    SELECT TOP 1
        @ScorePointsId = ScorePointsId,
        @KudosSent = KudosSent,
        @KudosReceived = KudosReceived,
        @LikesSent = LikesSent,
        @LikesReceived = LikesReceived,
        @CommentsSent = CommentsSent,
        @CommentsReceived = CommentsReceived
    FROM dbo.ScorePoints; 
	
	WITH UserPoints AS (



  SELECT
    KR.ToPersonId user_id,
    SUM(@KudosReceived) AS total_points
  FROM
    Kudos K   
    JOIN KudosReceiver KR
        ON K.KudosId = KR.KudosId
  WHERE
    KR.ToPersonId IS NOT NULL
  GROUP BY
    KR.ToPersonId

  UNION ALL
    SELECT
    frompersonid AS user_id,
    SUM(@KudosSent) AS total_points
  FROM
    Kudos k
        
  WHERE
    k.frompersonid IS NOT NULL
  GROUP BY
    frompersonid
UNION ALL
   
   SELECT
    PersonId,
    SUM(@LikesSent) AS total_points
  FROM
    KudosLike l
  
  WHERE
    l.PersonId IS NOT NULL
  GROUP BY
    PersonId

  UNION ALL

  SELECT
        KR.toPersonId AS user_id,
    SUM(@LikesReceived) AS total_points
    FROM
        DBO.KUDOS K
    JOIN KUDOSLIKE KL
        ON K.KUDOSID = KL.KUDOSID
    
    JOIN KudosReceiver KR
        ON K.KudosId = KR.KudosId
  WHERE
    KR.ToPersonId IS NOT NULL
  GROUP BY
    KR.toPersonId

  UNION ALL
 
  SELECT
    FromPersonId,
    SUM(@CommentsSent) AS total_points
  FROM
    Comments r
    
  WHERE
    r.FromPersonId IS NOT NULL
  GROUP BY
    FromPersonId

    UNION ALL
    
    SELECT
        KR.toPersonId AS user_id,
    SUM(@CommentsReceived) AS total_points
    FROM
        DBO.KUDOS K
    JOIN Comments KC
        ON K.KUDOSID = KC.KUDOSID
   
    JOIN KudosReceiver KR
        ON K.KudosId = KR.KudosId
    WHERE
        KR.toPersonId IS NOT NULL     
  GROUP BY
    KR.toPersonId


)
SELECT
up.user_id as UserId,
p.DisplayName,
ISNULL(p.Photo96x96,'') Photo,
SUM(up.total_points) AS TotalPoints
FROM
UserPoints up 
INNER JOIN UserProfiles p
 on up.user_id = p.UserProfileId
WHERE (@ManagerId is null or p.ManagerId = @ManagerId)
GROUP BY
  up.user_id,p.DisplayName,
p.Photo96x96
ORDER BY
  TotalPoints DESC
GO

GRANT EXECUTE ON OBJECT::dbo.SP_GETTOPCONTRIBUTORS TO superkudoskudos;


