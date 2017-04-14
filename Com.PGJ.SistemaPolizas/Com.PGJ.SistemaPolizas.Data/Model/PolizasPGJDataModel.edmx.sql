
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/14/2017 09:10:10
-- Generated from EDMX file: C:\GitRepos\Federico\PGJ\back-polizas\Com.PGJ.SistemaPolizas\Com.PGJ.SistemaPolizas.Data\Model\PolizasPGJDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PGJSistemaPolizas];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AfianzadoraPoliza]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Polizas] DROP CONSTRAINT [FK_AfianzadoraPoliza];
GO
IF OBJECT_ID(N'[dbo].[FK_AfianzadoDepositante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Depositantes] DROP CONSTRAINT [FK_AfianzadoDepositante];
GO
IF OBJECT_ID(N'[dbo].[FK_AfianzadoPoliza]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Afianzados] DROP CONSTRAINT [FK_AfianzadoPoliza];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaDetalleUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetallesUsuarios] DROP CONSTRAINT [FK_AreaDetalleUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_AutoridadEgreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Egresos] DROP CONSTRAINT [FK_AutoridadEgreso];
GO
IF OBJECT_ID(N'[dbo].[FK_AutoridadMinisterioPublico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MinisteriosPublicos] DROP CONSTRAINT [FK_AutoridadMinisterioPublico];
GO
IF OBJECT_ID(N'[dbo].[FK_DepositanteIngreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingresos] DROP CONSTRAINT [FK_DepositanteIngreso];
GO
IF OBJECT_ID(N'[dbo].[FK_DetalleUsuarioEgreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Egresos] DROP CONSTRAINT [FK_DetalleUsuarioEgreso];
GO
IF OBJECT_ID(N'[dbo].[FK_DetalleUsuarioIngreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingresos] DROP CONSTRAINT [FK_DetalleUsuarioIngreso];
GO
IF OBJECT_ID(N'[dbo].[FK_EgresoMinisterioPublico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MinisteriosPublicos] DROP CONSTRAINT [FK_EgresoMinisterioPublico];
GO
IF OBJECT_ID(N'[dbo].[FK_PolizaIngreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingresos] DROP CONSTRAINT [FK_PolizaIngreso];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Afianzadoras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Afianzadoras];
GO
IF OBJECT_ID(N'[dbo].[Afianzados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Afianzados];
GO
IF OBJECT_ID(N'[dbo].[Areas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Areas];
GO
IF OBJECT_ID(N'[dbo].[Autoridads]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Autoridads];
GO
IF OBJECT_ID(N'[dbo].[Depositantes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Depositantes];
GO
IF OBJECT_ID(N'[dbo].[DetallesUsuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetallesUsuarios];
GO
IF OBJECT_ID(N'[dbo].[Egresos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Egresos];
GO
IF OBJECT_ID(N'[dbo].[Ingresos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingresos];
GO
IF OBJECT_ID(N'[dbo].[MinisteriosPublicos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MinisteriosPublicos];
GO
IF OBJECT_ID(N'[dbo].[Polizas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Polizas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Afianzadoras'
CREATE TABLE [dbo].[Afianzadoras] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Afianzados'
CREATE TABLE [dbo].[Afianzados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [ApellidoPaterno] nvarchar(max)  NOT NULL,
    [ApellidoMaterno] nvarchar(max)  NOT NULL,
    [FechaDeNacimiento] datetime  NOT NULL
);
GO

-- Creating table 'Areas'
CREATE TABLE [dbo].[Areas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Autoridads'
CREATE TABLE [dbo].[Autoridads] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Depositantes'
CREATE TABLE [dbo].[Depositantes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [ApellidoMaterno] nvarchar(max)  NOT NULL,
    [ApellidoPaterno] nvarchar(max)  NOT NULL,
    [AfianzadoId] int  NOT NULL
);
GO

-- Creating table 'DetallesUsuarios'
CREATE TABLE [dbo].[DetallesUsuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [ApellidoMaterno] nvarchar(max)  NOT NULL,
    [ApellidoPaterno] nvarchar(max)  NOT NULL,
    [FechaDeNacimiento] datetime  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [AreaId] int  NOT NULL,
    [NumeroDeEmpleado] int  NOT NULL,
    [AuthUserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Egresos'
CREATE TABLE [dbo].[Egresos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AutoridadId] int  NOT NULL,
    [FechaDeEgreso] datetime  NOT NULL,
    [Cantidad] decimal(18,0)  NOT NULL,
    [DetalleUsuarioId] int  NOT NULL,
    [Descripcion] nvarchar(max)  NULL
);
GO

-- Creating table 'Ingresos'
CREATE TABLE [dbo].[Ingresos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cantidad] decimal(18,0)  NOT NULL,
    [PolizaId] int  NOT NULL,
    [FechaDeIngreso] datetime  NOT NULL,
    [DepositanteId] int  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [DetalleUsuarioId] int  NOT NULL
);
GO

-- Creating table 'MinisteriosPublicos'
CREATE TABLE [dbo].[MinisteriosPublicos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [AutoridadId] int  NULL,
    [EgresoId] int  NULL
);
GO

-- Creating table 'Polizas'
CREATE TABLE [dbo].[Polizas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AveriguacionPrevia] nvarchar(max)  NOT NULL,
    [AfianzadoraId] int  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [FechaDeAlta] datetime  NOT NULL,
    [Afianzado_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Afianzadoras'
ALTER TABLE [dbo].[Afianzadoras]
ADD CONSTRAINT [PK_Afianzadoras]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Afianzados'
ALTER TABLE [dbo].[Afianzados]
ADD CONSTRAINT [PK_Afianzados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [PK_Areas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Autoridads'
ALTER TABLE [dbo].[Autoridads]
ADD CONSTRAINT [PK_Autoridads]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Depositantes'
ALTER TABLE [dbo].[Depositantes]
ADD CONSTRAINT [PK_Depositantes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DetallesUsuarios'
ALTER TABLE [dbo].[DetallesUsuarios]
ADD CONSTRAINT [PK_DetallesUsuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Egresos'
ALTER TABLE [dbo].[Egresos]
ADD CONSTRAINT [PK_Egresos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ingresos'
ALTER TABLE [dbo].[Ingresos]
ADD CONSTRAINT [PK_Ingresos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MinisteriosPublicos'
ALTER TABLE [dbo].[MinisteriosPublicos]
ADD CONSTRAINT [PK_MinisteriosPublicos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Polizas'
ALTER TABLE [dbo].[Polizas]
ADD CONSTRAINT [PK_Polizas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AfianzadoraId] in table 'Polizas'
ALTER TABLE [dbo].[Polizas]
ADD CONSTRAINT [FK_AfianzadoraPoliza]
    FOREIGN KEY ([AfianzadoraId])
    REFERENCES [dbo].[Afianzadoras]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AfianzadoraPoliza'
CREATE INDEX [IX_FK_AfianzadoraPoliza]
ON [dbo].[Polizas]
    ([AfianzadoraId]);
GO

-- Creating foreign key on [AfianzadoId] in table 'Depositantes'
ALTER TABLE [dbo].[Depositantes]
ADD CONSTRAINT [FK_AfianzadoDepositante]
    FOREIGN KEY ([AfianzadoId])
    REFERENCES [dbo].[Afianzados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AfianzadoDepositante'
CREATE INDEX [IX_FK_AfianzadoDepositante]
ON [dbo].[Depositantes]
    ([AfianzadoId]);
GO

-- Creating foreign key on [AreaId] in table 'DetallesUsuarios'
ALTER TABLE [dbo].[DetallesUsuarios]
ADD CONSTRAINT [FK_AreaDetalleUsuario]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaDetalleUsuario'
CREATE INDEX [IX_FK_AreaDetalleUsuario]
ON [dbo].[DetallesUsuarios]
    ([AreaId]);
GO

-- Creating foreign key on [AutoridadId] in table 'Egresos'
ALTER TABLE [dbo].[Egresos]
ADD CONSTRAINT [FK_AutoridadEgreso]
    FOREIGN KEY ([AutoridadId])
    REFERENCES [dbo].[Autoridads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AutoridadEgreso'
CREATE INDEX [IX_FK_AutoridadEgreso]
ON [dbo].[Egresos]
    ([AutoridadId]);
GO

-- Creating foreign key on [AutoridadId] in table 'MinisteriosPublicos'
ALTER TABLE [dbo].[MinisteriosPublicos]
ADD CONSTRAINT [FK_AutoridadMinisterioPublico]
    FOREIGN KEY ([AutoridadId])
    REFERENCES [dbo].[Autoridads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AutoridadMinisterioPublico'
CREATE INDEX [IX_FK_AutoridadMinisterioPublico]
ON [dbo].[MinisteriosPublicos]
    ([AutoridadId]);
GO

-- Creating foreign key on [DepositanteId] in table 'Ingresos'
ALTER TABLE [dbo].[Ingresos]
ADD CONSTRAINT [FK_DepositanteIngreso]
    FOREIGN KEY ([DepositanteId])
    REFERENCES [dbo].[Depositantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepositanteIngreso'
CREATE INDEX [IX_FK_DepositanteIngreso]
ON [dbo].[Ingresos]
    ([DepositanteId]);
GO

-- Creating foreign key on [DetalleUsuarioId] in table 'Egresos'
ALTER TABLE [dbo].[Egresos]
ADD CONSTRAINT [FK_DetalleUsuarioEgreso]
    FOREIGN KEY ([DetalleUsuarioId])
    REFERENCES [dbo].[DetallesUsuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DetalleUsuarioEgreso'
CREATE INDEX [IX_FK_DetalleUsuarioEgreso]
ON [dbo].[Egresos]
    ([DetalleUsuarioId]);
GO

-- Creating foreign key on [DetalleUsuarioId] in table 'Ingresos'
ALTER TABLE [dbo].[Ingresos]
ADD CONSTRAINT [FK_DetalleUsuarioIngreso]
    FOREIGN KEY ([DetalleUsuarioId])
    REFERENCES [dbo].[DetallesUsuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DetalleUsuarioIngreso'
CREATE INDEX [IX_FK_DetalleUsuarioIngreso]
ON [dbo].[Ingresos]
    ([DetalleUsuarioId]);
GO

-- Creating foreign key on [EgresoId] in table 'MinisteriosPublicos'
ALTER TABLE [dbo].[MinisteriosPublicos]
ADD CONSTRAINT [FK_EgresoMinisterioPublico]
    FOREIGN KEY ([EgresoId])
    REFERENCES [dbo].[Egresos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EgresoMinisterioPublico'
CREATE INDEX [IX_FK_EgresoMinisterioPublico]
ON [dbo].[MinisteriosPublicos]
    ([EgresoId]);
GO

-- Creating foreign key on [PolizaId] in table 'Ingresos'
ALTER TABLE [dbo].[Ingresos]
ADD CONSTRAINT [FK_PolizaIngreso]
    FOREIGN KEY ([PolizaId])
    REFERENCES [dbo].[Polizas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolizaIngreso'
CREATE INDEX [IX_FK_PolizaIngreso]
ON [dbo].[Ingresos]
    ([PolizaId]);
GO

-- Creating foreign key on [Afianzado_Id] in table 'Polizas'
ALTER TABLE [dbo].[Polizas]
ADD CONSTRAINT [FK_PolizasAfianzados]
    FOREIGN KEY ([Afianzado_Id])
    REFERENCES [dbo].[Afianzados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolizasAfianzados'
CREATE INDEX [IX_FK_PolizasAfianzados]
ON [dbo].[Polizas]
    ([Afianzado_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------