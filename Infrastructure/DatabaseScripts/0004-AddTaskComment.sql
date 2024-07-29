create table if not exists public.tasks_comment
(
    id                 serial primary key                        not null,
    content            text                                      not null,
    task_id            int                                       not null
        constraint tasks_projects_id_fkey
            references public.tasks
            on delete cascade,
    created_at         timestamp without time zone default now() not null,
    updated_at         timestamp without time zone,
    deleted_at         timestamp without time zone,
    created_by_user_id integer,
    updated_by_user_id integer,
    deleted_by_user_id integer
);
