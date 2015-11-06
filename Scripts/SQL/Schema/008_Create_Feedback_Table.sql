
USE [PeninsulaPortal]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Feedback](
                [Id] [bigint] IDENTITY(1,1) NOT NULL,
                [DateSubmitted] [datetime] NOT NULL,                                
                [PortalUserID] [nvarchar](128) NOT NULL,
                
                [PageSubmittedFrom] [nvarchar](512) NOT NULL,
                
                [FeedbackText] [nvarchar](MAX) NOT NULL,
                
CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
    
ALTER TABLE dbo.[Feedback]   
    ADD CONSTRAINT FK_Feedback_Users
            FOREIGN KEY (PortalUserID) REFERENCES AspNetUsers(Id);


