create table score(id bigserial primary key, cpf varchar(255) not null, score int not null, created_at timestamp with time zone not null);
create index score_main_idx ON score(cpf, created_at);
create index score_cpf_idx ON score(cpf);
create index score_id_idx ON score(id);
