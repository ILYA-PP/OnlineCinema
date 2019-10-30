

CREATE TABLE [dbo].[Актёр] (
    [КодАктёра] INT           NOT NULL,
    [Фамилия]   VARCHAR (100) NULL,
    [Имя]       VARCHAR (100) NOT NULL,
    [Отчество]  VARCHAR (100) NULL,
    CONSTRAINT [PK_Актёр] PRIMARY KEY CLUSTERED ([КодАктёра] ASC)
);

CREATE TABLE [dbo].[АктёрыКонтента] (
    [КодАктёра]   INT NOT NULL,
    [КодКонтента] INT NOT NULL,
    CONSTRAINT [FK_АктёрыКонтента_Актёр] FOREIGN KEY ([КодАктёра]) REFERENCES [dbo].[Актёр] ([КодАктёра]),
    CONSTRAINT [FK_АктёрыКонтента_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента])
);

CREATE TABLE [dbo].[Файл] (
    [КодФайла] INT           NOT NULL,
    [Ссылка]   VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Файл] PRIMARY KEY CLUSTERED ([КодФайла] ASC)
);

CREATE TABLE [dbo].[ФайлыКонтента] (
    [КодФайла]    INT NOT NULL,
    [КодКонтента] INT NOT NULL,
    CONSTRAINT [FK_ФайлыКонтента_Файл] FOREIGN KEY ([КодФайла]) REFERENCES [dbo].[Файл] ([КодФайла]),
    CONSTRAINT [FK_ФайлыКонтента_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента])
);

CREATE TABLE [dbo].[Страна] (
    [КодСтраны]    INT           NOT NULL,
    [Наименование] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Страна] PRIMARY KEY CLUSTERED ([КодСтраны] ASC)
);

CREATE TABLE [dbo].[СтраныКонтента] (
    [КодСтраны]   INT NOT NULL,
    [КодКонтента] INT NOT NULL,
    CONSTRAINT [FK_СтраныКонтента_Страна] FOREIGN KEY ([КодСтраны]) REFERENCES [dbo].[Страна] ([КодСтраны]),
    CONSTRAINT [FK_СтраныКонтента_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента])
);

CREATE TABLE [dbo].[Режиссёр] (
    [КодРежиссёра] INT           NOT NULL,
    [Фамилия]      VARCHAR (100) NULL,
    [Имя]          VARCHAR (100) NOT NULL,
    [Отчество]     VARCHAR (100) NULL,
    CONSTRAINT [PK_Режиссёр] PRIMARY KEY CLUSTERED ([КодРежиссёра] ASC)
);

CREATE TABLE [dbo].[РежиссёрыКонтента] (
    [КодРежиссёра] INT NOT NULL,
    [КодКонтента]  INT NOT NULL,
    CONSTRAINT [FK_РежиссёрыКонтента_Режиссёр] FOREIGN KEY ([КодРежиссёра]) REFERENCES [dbo].[Режиссёр] ([КодРежиссёра]),
    CONSTRAINT [FK_РежиссёрыКонтента_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента])
);

CREATE TABLE [dbo].[Отзыв] (
    [КодОтзыва]   INT           NOT NULL,
    [КодКонтента] INT           NOT NULL,
    [Логин]       VARCHAR (50)  NOT NULL,
    [Текст]       VARCHAR (MAX) NOT NULL,
    [Фильм]       BIT           NOT NULL,
    CONSTRAINT [PK_Отзыв] PRIMARY KEY CLUSTERED ([КодОтзыва] ASC),
    CONSTRAINT [FK_Отзыв_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента]),
    CONSTRAINT [FK_Отзыв_Пользователь] FOREIGN KEY ([Логин]) REFERENCES [dbo].[Пользователь] ([Логин])
);


CREATE TABLE [dbo].[Заказ] (
    [КодЗаказа]   INT          NOT NULL,
    [КодКонтента] INT          NOT NULL,
    [Логин]       VARCHAR (50) NOT NULL,
    [Стоимость]   MONEY        NOT NULL,
    CONSTRAINT [PK_Заказ] PRIMARY KEY CLUSTERED ([КодЗаказа] ASC),
    CONSTRAINT [FK_Заказ_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента]),
    CONSTRAINT [FK_Заказ_Пользователь] FOREIGN KEY ([Логин]) REFERENCES [dbo].[Пользователь] ([Логин])
);

CREATE TABLE [dbo].[Жанр] (
    [КодЖанра]     INT           NOT NULL,
    [Наименование] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Жанр] PRIMARY KEY CLUSTERED ([КодЖанра] ASC)
);

CREATE TABLE [dbo].[ЖанрКонтента] (
    [КодЖанра]    INT NOT NULL,
    [КодКонтента] INT NOT NULL,
    CONSTRAINT [FK_ЖанрКонтента_Жанр] FOREIGN KEY ([КодЖанра]) REFERENCES [dbo].[Жанр] ([КодЖанра]),
    CONSTRAINT [FK_ЖанрКонтента_Контент] FOREIGN KEY ([КодКонтента]) REFERENCES [dbo].[Контент] ([КодКонтента])
);
