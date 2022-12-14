USE [master]
GO

CREATE DATABASE [Cdemo]
GO
USE [Cdemo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerNoteStates](
	[Dbid] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[AuthorUserId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Text] [nvarchar](800) NOT NULL,
 CONSTRAINT [PK_CustomerNoteStates] PRIMARY KEY CLUSTERED 
(
	[Dbid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerStates](
	[Dbid] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RegistrationDateTime] [datetime] NOT NULL,
	[ResponsibleUserId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](80) NOT NULL,
	[LastName] [nvarchar](80) NOT NULL,
	[Phone] [nvarchar](40) NULL,
	[Email] [nvarchar](40) NULL,
 CONSTRAINT [PK_CustomerStates] PRIMARY KEY CLUSTERED 
(
	[Dbid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeStates](
	[Dbid] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](80) NOT NULL,
	[LastName] [nvarchar](80) NOT NULL,
 CONSTRAINT [PK_EmployeeStates] PRIMARY KEY CLUSTERED 
(
	[Dbid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStates](
	[Dbid] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[PassHash] [nvarchar](80) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_UserStates] PRIMARY KEY CLUSTERED 
(
	[Dbid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserStates] ADD  CONSTRAINT [DF_UserStates_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
