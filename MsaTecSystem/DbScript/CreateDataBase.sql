-- Database: MsatecDb

-- DROP DATABASE IF EXISTS "MsatecDb";

CREATE DATABASE "MsatecDb"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
	
-- Table: public.Clientes

-- DROP TABLE IF EXISTS public."Clientes";

CREATE TABLE IF NOT EXISTS public."Clientes"
(
    "Id" uuid NOT NULL,
    "Nome" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "DataNascimento" date,
    "CriadoEm" timestamp with time zone NOT NULL DEFAULT CURRENT_DATE,
    "AtualzadoEm" timestamp with time zone,
    CONSTRAINT "PK_Clientes" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Clientes"
    OWNER to postgres;
-- Index: IX_Clientes_Email

-- DROP INDEX IF EXISTS public."IX_Clientes_Email";

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Clientes_Email"
    ON public."Clientes" USING btree
    ("Email" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;

-- Table: public.Telefones

-- DROP TABLE IF EXISTS public."Telefones";

CREATE TABLE IF NOT EXISTS public."Telefones"
(
    "Id" uuid NOT NULL,
    "Numero" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "Tipo" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "ClienteId" uuid NOT NULL,
    "CriadoEm" timestamp with time zone NOT NULL DEFAULT CURRENT_DATE,
    "AtualzadoEm" timestamp with time zone,
    CONSTRAINT "PK_Telefones" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Telefones_Clientes_ClienteId" FOREIGN KEY ("ClienteId")
        REFERENCES public."Clientes" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

-- Table: public.Telefones

-- DROP TABLE IF EXISTS public."Telefones";

CREATE TABLE IF NOT EXISTS public."Telefones"
(
    "Id" uuid NOT NULL,
    "Numero" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "Tipo" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "ClienteId" uuid NOT NULL,
    "CriadoEm" timestamp with time zone NOT NULL DEFAULT CURRENT_DATE,
    "AtualzadoEm" timestamp with time zone,
    CONSTRAINT "PK_Telefones" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Telefones_Clientes_ClienteId" FOREIGN KEY ("ClienteId")
        REFERENCES public."Clientes" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Telefones"
    OWNER to postgres;
-- Index: IX_Telefones_ClienteId

-- DROP INDEX IF EXISTS public."IX_Telefones_ClienteId";

CREATE INDEX IF NOT EXISTS "IX_Telefones_ClienteId"
    ON public."Telefones" USING btree
    ("ClienteId" ASC NULLS LAST)
    TABLESPACE pg_default;