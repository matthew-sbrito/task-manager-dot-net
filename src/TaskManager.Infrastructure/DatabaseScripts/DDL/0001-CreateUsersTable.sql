create table if not exists public.users
(
    id                 serial primary key                        not null,
    name               varchar(255)                              not null,
    role               int                                       not null,
    created_at         timestamp without time zone default now() not null,
    updated_at         timestamp without time zone,
    deleted_at         timestamp without time zone,
    created_by_user_id integer
        constraint users_created_by_user_id_fkey
            references public.users on delete set null,
    updated_by_user_id integer
        constraint users_updated_by_user_id_fkey
            references public.users on delete set null,
    deleted_by_user_id integer
        constraint users_deleted_by_user_id_fkey
            references public.users on delete set null
);
