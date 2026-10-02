# DirectFNCRMKurum
DB’ye yapılan eklemelerdeki SQL Scripti 

-- AnProLisansı için Sutun Ekleme
ALTER TABLE [infotradeDB].[dbo].[LisansDurum]
ADD [AnPro] [bit] DEFAULT ((0)) NOT NULL,
[AnProStart] [date] NULL,
[AnProEnd] [date] NULL

--Gün Sonu Toplamlar
ALTER TABLE [CrmiDealDB].[dbo].GunSonuToplamlar
ADD [AnPro] [int] NULL



