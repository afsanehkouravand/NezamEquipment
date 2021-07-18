--SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [AreaType]) VALUES ('c46ed2e9-9ec8-4023-8301-f372685753f2', N'sa', N'sa@gmail.com', 1, N'AN5gEU4i4ReYFLYs9PdEQy6RsvN6MCVCVEWm8yWUjc3IrVOhcQDHrDZI2KX9revx5Q==', N'f348cb31-5508-40df-82fd-77f7a0d16e6c', NULL, 1, 1, NULL, 1, 0, 0)
--SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF 
--SET IDENTITY_INSERT [dbo].[AspNetRoles] ON 
INSERT [dbo].[AspNetRoles] ([Id], [Name], [AreaType]) VALUES ('f791f150-9b64-42a0-bf9a-615245659070', N'ادمین', 0)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [AreaType]) VALUES ('1394a057-3cee-46a3-b3f9-e2c59792b718', N'کاربر اخراجی', 0)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [AreaType]) VALUES ('081a0e16-c0e9-4965-98bf-256d01da514d', N'ارزیاب', 0)
--SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('c46ed2e9-9ec8-4023-8301-f372685753f2', 'f791f150-9b64-42a0-bf9a-615245659070')
