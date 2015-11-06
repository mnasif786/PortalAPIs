
USE [PeninsulaPortal]
GO

/****** Object:  Table [dbo].[UserProfiles]    Script Date: 08/19/2015 16:05:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserProfiles](
                [Id] [bigint] IDENTITY(1,1) NOT NULL,
                [FirstName] [nvarchar](250) NOT NULL,
                [MiddleName] [nvarchar](250) NULL,
                [LastName] [nvarchar](250) NOT NULL,
                [Initials] [nvarchar](150) NULL,
                [DateOfBirth] [date] NULL,
                [CreatedDate] [datetime] NOT NULL,
CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
                [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE dbo.AspNetUsers
    ADD ProfileId BIGINT NULL
    
ALTER TABLE dbo.AspNetUsers   
    ADD CONSTRAINT FK_AspNetUsers_UserProfiles
            FOREIGN KEY (ProfileId) REFERENCES UserProfiles(Id);
