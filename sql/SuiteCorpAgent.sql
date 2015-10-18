USE [IsvAuth]
GO

/****** Object:  Table [dbo].[SuiteCorpAgent]    Script Date: 10/16/2015 16:20:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuiteCorpAgent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SuiteKey] [nvarchar](50) NULL,
	[CorpId] [nvarchar](100) NULL,
	[AgentId] [nvarchar](100) NULL,
	[AgentName] [nvarchar](100) NULL,
	[LogoUrl] [nvarchar](100) NULL,
	[Appid] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
	[IsClose] [int] NULL,
 CONSTRAINT [PK_SuiteCorpAgent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

