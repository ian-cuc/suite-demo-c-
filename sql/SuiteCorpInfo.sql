USE [IsvAuth]
GO

/****** Object:  Table [dbo].[SuiteCorpInfo]    Script Date: 10/16/2015 16:20:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuiteCorpInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SuiteKey] [nvarchar](50) NULL,
	[TmpAuthCode] [nvarchar](50) NULL,
	[PermanentCode] [nvarchar](100) NULL,
	[Corpid] [nvarchar](50) NULL,
	[CorpName] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[CorpLogoUrl] [nvarchar](100) NULL,
	[AccessToken] [nvarchar](100) NULL,
	[TokenExpires] [datetime] NULL,
 CONSTRAINT [PK_SuiteAuthCorpInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

