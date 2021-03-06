USE [FigKey_Logger]
GO
/****** Object:  Table [dbo].[f_user]    Script Date: 04/30/2019 18:24:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[f_user](
	[user_id] [int] NOT NULL,
	[username] [nchar](10) NOT NULL,
	[password] [nchar](10) NOT NULL,
	[phone] [nchar](20) NULL,
	[email] [nchar](30) NULL,
	[picture] [binary](50) NULL,
	[create_date] [datetime] NULL,
	[last_update_date] [datetime] NULL,
	[status] [int] NULL,
	[user_type] [int] NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-离线，1-在线' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'f_user', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-普通用户，1-管理员，2-游客' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'f_user', @level2type=N'COLUMN',@level2name=N'user_type'
GO
/****** Object:  Default [DF_f_user_status]    Script Date: 04/30/2019 18:24:09 ******/
ALTER TABLE [dbo].[f_user] ADD  CONSTRAINT [DF_f_user_status]  DEFAULT ((0)) FOR [status]
GO
/****** Object:  Default [DF_f_user_user_type]    Script Date: 04/30/2019 18:24:09 ******/
ALTER TABLE [dbo].[f_user] ADD  CONSTRAINT [DF_f_user_user_type]  DEFAULT ((0)) FOR [user_type]
GO
