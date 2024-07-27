create table if not exists public.project
(
    id          serial primary key not null,
    title       varchar(55)        not null,
    description varchar(255)       not null
    );

create table if not exists public.task
(
    id          serial primary key not null,
    title       varchar(55)        not null,
    description varchar(255)       not null
);