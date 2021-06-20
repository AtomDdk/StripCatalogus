CREATE TABLE [dbo].[Uitgeverij]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Naam] NVARCHAR(100) NOT NULL UNIQUE
)
CREATE TABLE [dbo].[Auteur]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Naam] NVARCHAR(100) UNIQUE
)
CREATE TABLE [dbo].[Reeks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Naam] NVARCHAR(100) NOT NULL UNIQUE, 
)
CREATE TABLE [dbo].[Strip]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Titel] NVARCHAR(100) NOT NULL, 
    [ReeksId] INT NULL, 
    [ReeksNummer] INT NULL,
	[UitgeverijId] INT NOT NULL,
	CONSTRAINT [FK_Strip_to_Uitgeverij] FOREIGN KEY([UitgeverijId]) REFERENCES [dbo].[Uitgeverij]([Id]),
	CONSTRAINT [FK_Strip_to_Reeks] FOREIGN KEY([ReeksId]) REFERENCES [dbo].[Reeks]([Id]),
)
CREATE TABLE [dbo].[StripAuteur]
(
	[StripId] INT NOT NULL,
	[AuteurId] INT NOT NULL,
	PRIMARY KEY(StripId,AuteurId),
	Constraint [FK_StripAuteur_to_Strip] FOREIGN KEY ([StripId]) REFERENCES [dbo].[Strip]([Id]),
	CONSTRAINT [FK_StripAuteur_to_Auteur] FOREIGN KEY ([AuteurId]) REFERENCES [dbo].[Auteur]([Id])
)
CREATE TABLE [dbo].[StripAantal]
(
	[StripId] INT NOT NULL,
	[Aantal] INT,
	PRIMARY KEY(StripId,Aantal),
	Constraint [FK_StripAantal_to_Strip] FOREIGN KEY ([StripId]) REFERENCES [dbo].[Strip]([Id])
)
CREATE TABLE [dbo].[Bestelling]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Datum] DATETIME
)
CREATE TABLE [dbo].[Levering]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[BestelDatum] DATETIME,
	[LeverDatum] DATETIME
)
CREATE TABLE [dbo].[StripLevering]
(
	[StripId] INT NOT NULL,
	[LeveringId] INT NOT NULL,
	[Aantal] INT,
	Constraint [FK_StripLevering_to_Strip] FOREIGN KEY ([StripId]) REFERENCES [dbo].[Strip]([Id]),
	CONSTRAINT [FK_StripLevering_to_Levering] FOREIGN KEY ([LeveringId]) REFERENCES [dbo].[Levering]([Id])
)
CREATE TABLE [dbo].[StripBestelling]
(
	[StripId] INT NOT NULL,
	[BestellingId] INT NOT NULL,
	[Aantal] INT,
	Constraint [FK_StripBestelling_to_Strip] FOREIGN KEY ([StripId]) REFERENCES [dbo].[Strip]([Id]),
	CONSTRAINT [FK_StripBestelling_to_Bestelling] FOREIGN KEY ([BestellingId]) REFERENCES [dbo].[Bestelling]([Id])
)