USE [master]
GO
/****** Object:  Database [AuthorizationDBForAzureAD]    Script Date: 4/14/2023 5:57:24 AM ******/
CREATE DATABASE [AuthorizationDBForAzureAD]
 
GO

USE [AuthorizationDBForAzureAD]
GO
/****** Object:  Table [dbo].[SolutionRoles]    Script Date: 4/14/2023 5:57:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolutionRoles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SolutionRoles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SolutionUsers]    Script Date: 4/14/2023 5:57:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolutionUsers](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [nvarchar](70) NOT NULL,
 CONSTRAINT [PK_SolutionUsers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SolutionUsersInRoles]    Script Date: 4/14/2023 5:57:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolutionUsersInRoles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_SolutionUsersInRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SolutionRoles] ON 
GO
INSERT [dbo].[SolutionRoles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
GO
INSERT [dbo].[SolutionRoles] ([RoleID], [RoleName]) VALUES (2, N'AppUser')
GO
SET IDENTITY_INSERT [dbo].[SolutionRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[SolutionUsers] ON 
GO
INSERT [dbo].[SolutionUsers] ([UserID], [EmailAddress]) VALUES (2, N'AdeleV@zzq7l.onmicrosoft.com')
GO
INSERT [dbo].[SolutionUsers] ([UserID], [EmailAddress]) VALUES (1, N'benjaminsqlserver@zzq7l.onmicrosoft.com')
GO
SET IDENTITY_INSERT [dbo].[SolutionUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[SolutionUsersInRoles] ON 
GO
INSERT [dbo].[SolutionUsersInRoles] ([ID], [UserID], [RoleID]) VALUES (1, 1, 1)
GO
INSERT [dbo].[SolutionUsersInRoles] ([ID], [UserID], [RoleID]) VALUES (2, 2, 2)
GO
SET IDENTITY_INSERT [dbo].[SolutionUsersInRoles] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SolutionRoles]    Script Date: 4/14/2023 5:57:25 AM ******/
ALTER TABLE [dbo].[SolutionRoles] ADD  CONSTRAINT [IX_SolutionRoles] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SolutionUsers]    Script Date: 4/14/2023 5:57:25 AM ******/
ALTER TABLE [dbo].[SolutionUsers] ADD  CONSTRAINT [IX_SolutionUsers] UNIQUE NONCLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SolutionUsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_SolutionUsersInRoles_SolutionRoles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[SolutionRoles] ([RoleID])
GO
ALTER TABLE [dbo].[SolutionUsersInRoles] CHECK CONSTRAINT [FK_SolutionUsersInRoles_SolutionRoles]
GO
ALTER TABLE [dbo].[SolutionUsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_SolutionUsersInRoles_SolutionUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[SolutionUsers] ([UserID])
GO
ALTER TABLE [dbo].[SolutionUsersInRoles] CHECK CONSTRAINT [FK_SolutionUsersInRoles_SolutionUsers]
GO
/****** Object:  StoredProcedure [dbo].[FetchRolesForADUser]    Script Date: 4/14/2023 5:57:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchRolesForADUser]
@Username nvarchar(70)
AS
SELECT        SolutionRoles.RoleName
FROM            SolutionUsers INNER JOIN
                         SolutionUsersInRoles ON SolutionUsers.UserID = SolutionUsersInRoles.UserID INNER JOIN
                         SolutionRoles ON SolutionUsersInRoles.RoleID = SolutionRoles.RoleID
						 WHERE SolutionUsers.EmailAddress=@Username
GO
USE [master]
GO
ALTER DATABASE [AuthorizationDBForAzureAD] SET  READ_WRITE 
GO
