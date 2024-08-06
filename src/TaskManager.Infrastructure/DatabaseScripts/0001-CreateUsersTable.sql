create table if not exists public.users
(
    id                 serial primary key                        not null,
    name               varchar(255)                              not null,
    role               int                                       not null,
    created_at         timestamp without time zone default now() not null,
    updated_at         timestamp without time zone,
    deleted_at         timestamp without time zone,
    created_by_user_id integer,
    updated_by_user_id integer,
    deleted_by_user_id integer
);
