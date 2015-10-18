USE [IsvAuth]
GO

/****** Object:  Table [dbo].[SuiteKeyInfo]    Script Date: 10/16/2015 16:21:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuiteKeyInfo](
	[SuiteKey] [nvarchar](50) NOT NULL,
	[SuiteSecret] [nvarchar](100) NOT NULL,
	[SuiteTicket] [nvarchar](100) NULL,
	[TimeStamp] [nvarchar](50) NULL,
	[SuiteToken] [nvarchar](100) NULL,
	[SuiteTokenExpires] [datetime] NULL,
 CONSTRAINT [PK_SuiteInfo] PRIMARY KEY CLUSTERED 
(
	[SuiteKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

