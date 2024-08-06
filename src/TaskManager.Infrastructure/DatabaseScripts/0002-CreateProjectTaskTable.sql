create table if not exists public.projects
(
    id                 serial primary key                        not null,
    title              varchar(55)                               not null,
    description        varchar(255)                              not null,
    created_at         timestamp without time zone default now() not null,
    updated_at         timestamp without time zone,
    deleted_at         timestamp without time zone,
    created_by_user_id integer,
    updated_by_user_id integer,
    deleted_by_user_id integer
);

create table if not exists public.tasks
(
    id                 serial primary key                        not null,
    title              varchar(55)                               not null,
    description        varchar(255)                              not null,
    status             int                                       not null,
    priority           int                                       not null,
    due_date           timestamp without time zone               not null,
    project_id         int                                       not null
        constraint tasks_projects_id_fkey
            references public.projects
            on delete cascade,
    created_at         timestamp without time zone default now() not null,
    updated_at         timestamp without time zone,
    deleted_at         timestamp without time zone,
    created_by_user_id integer,
    updated_by_user_id integer,
    deleted_by_user_id integer
);
