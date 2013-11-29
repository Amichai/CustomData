
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/29/2013 08:33:23
-- Generated from EDMX file: C:\Users\Amichai\Documents\Visual Studio 2010\Projects\ModelAndSimulations\CustomData\CustomDataSet\TaskCollectorModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TaskCollector];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Task_0]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_0];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskHit_0]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskHit] DROP CONSTRAINT [FK_TaskHit_0];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskHit_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskHit] DROP CONSTRAINT [FK_TaskHit_1];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTask_0]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTask] DROP CONSTRAINT [FK_UserTask_0];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTask_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTask] DROP CONSTRAINT [FK_UserTask_1];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTask_2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTask] DROP CONSTRAINT [FK_UserTask_2];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Task]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Task];
GO
IF OBJECT_ID(N'[dbo].[TaskHit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskHit];
GO
IF OBJECT_ID(N'[dbo].[TaskRelationships]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskRelationships];
GO
IF OBJECT_ID(N'[dbo].[TaskVisibility]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskVisibility];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UserTask]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTask];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [HitCount] int  NOT NULL,
    [CompletedAfter] int  NULL,
    [HitDisabled] bigint  NULL,
    [CompletionDisabled] bigint  NULL,
    [Visibility] int  NULL
);
GO

-- Creating table 'TaskHits'
CREATE TABLE [dbo].[TaskHits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Task] int  NOT NULL,
    [User] int  NOT NULL,
    [Timestamp] datetime  NOT NULL
);
GO

-- Creating table 'TaskRelationships'
CREATE TABLE [dbo].[TaskRelationships] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(100)  NULL
);
GO

-- Creating table 'TaskVisibilities'
CREATE TABLE [dbo].[TaskVisibilities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(100)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmailAddress] nvarchar(100)  NOT NULL,
    [Password] nvarchar(30)  NOT NULL,
    [CreationTime] datetime  NOT NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'UserTasks'
CREATE TABLE [dbo].[UserTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [User] int  NOT NULL,
    [Task] int  NOT NULL,
    [Relationship] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskHits'
ALTER TABLE [dbo].[TaskHits]
ADD CONSTRAINT [PK_TaskHits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskRelationships'
ALTER TABLE [dbo].[TaskRelationships]
ADD CONSTRAINT [PK_TaskRelationships]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskVisibilities'
ALTER TABLE [dbo].[TaskVisibilities]
ADD CONSTRAINT [PK_TaskVisibilities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserTasks'
ALTER TABLE [dbo].[UserTasks]
ADD CONSTRAINT [PK_UserTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Visibility] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Task_0]
    FOREIGN KEY ([Visibility])
    REFERENCES [dbo].[TaskVisibilities]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Task_0'
CREATE INDEX [IX_FK_Task_0]
ON [dbo].[Tasks]
    ([Visibility]);
GO

-- Creating foreign key on [Task] in table 'TaskHits'
ALTER TABLE [dbo].[TaskHits]
ADD CONSTRAINT [FK_TaskHit_0]
    FOREIGN KEY ([Task])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskHit_0'
CREATE INDEX [IX_FK_TaskHit_0]
ON [dbo].[TaskHits]
    ([Task]);
GO

-- Creating foreign key on [Task] in table 'UserTasks'
ALTER TABLE [dbo].[UserTasks]
ADD CONSTRAINT [FK_UserTask_1]
    FOREIGN KEY ([Task])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTask_1'
CREATE INDEX [IX_FK_UserTask_1]
ON [dbo].[UserTasks]
    ([Task]);
GO

-- Creating foreign key on [User] in table 'TaskHits'
ALTER TABLE [dbo].[TaskHits]
ADD CONSTRAINT [FK_TaskHit_1]
    FOREIGN KEY ([User])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskHit_1'
CREATE INDEX [IX_FK_TaskHit_1]
ON [dbo].[TaskHits]
    ([User]);
GO

-- Creating foreign key on [Relationship] in table 'UserTasks'
ALTER TABLE [dbo].[UserTasks]
ADD CONSTRAINT [FK_UserTask_2]
    FOREIGN KEY ([Relationship])
    REFERENCES [dbo].[TaskRelationships]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTask_2'
CREATE INDEX [IX_FK_UserTask_2]
ON [dbo].[UserTasks]
    ([Relationship]);
GO

-- Creating foreign key on [User] in table 'UserTasks'
ALTER TABLE [dbo].[UserTasks]
ADD CONSTRAINT [FK_UserTask_0]
    FOREIGN KEY ([User])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTask_0'
CREATE INDEX [IX_FK_UserTask_0]
ON [dbo].[UserTasks]
    ([User]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------