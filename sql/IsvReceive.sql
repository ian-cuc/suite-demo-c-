USE [IsvAuth]
GO

/****** Object:  Table [dbo].[IsvReceive]    Script Date: 10/16/2015 16:23:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IsvReceive](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Signature] [nvarchar](50) NULL,
	[Timestamp] [nvarchar](50) NULL,
	[Nonce] [nvarchar](50) NULL,
	[Encrypt] [nvarchar](500) NULL,
	[EchoString] [nvarchar](500) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_IsvReceive] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IsvReceive] ADD  CONSTRAINT [DF_IsvReceive_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

