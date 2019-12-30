--create database Movie_Database;
--use Movie_Database;

create table Movie (
	ID int primary key identity(1,1),
	TMDB_ID int not null,
	title nvarchar(256) not null,
	[description] nvarchar(max) not null,
	release_date date not null,
	runtime int not null check(runtime >= 0),
	rating decimal(2,1) not null check(rating >= 0.0 and rating <= 10.0),
	poster_path varchar(128),
	budget int check(budget >= 0),
)

create table Person (
	ID int primary key identity(1,1),
	TMDB_ID int not null,
	[name] nvarchar(256) not null,
	date_of_birth date not null check(date_of_birth < getdate()),
	place_of_birth varchar(256),
	gender int not null check(gender >= 0 and gender <= 2),
	photo_path varchar(128),
)

create table Actor_Movie (
	ID int primary key identity(1,1),
	[character] nvarchar(256) not null,
	person int not null foreign key references Person(ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Director_Movie (
	ID int primary key identity(1,1),
	person int not null foreign key references Person(ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Genre (
	ID int primary key identity(1,1),
	TMDB_ID int not null,
	title varchar(32) not null,
)

create table Genre_Movie (
	ID int primary key identity(1,1),
	genre int not null foreign key references Genre(ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table [Language] (
	ID int primary key identity(1,1),
	iso_code varchar(2),
	[name] nvarchar(64) not null,
)

create table Language_Movie (
	ID int primary key identity(1,1),
	[language] int not null foreign key references [Language](ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Country (
	ID int primary key identity(1,1),
	iso_code varchar(2),
	[name] nvarchar(64) not null,
)

create table Country_Movie (
	ID int primary key identity(1,1),
	country int not null foreign key references Country(ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Review (
	ID int primary key identity(1,1),
	[description] nvarchar(max) not null,
	date_created date not null default getdate(),
	author nvarchar(124) not null default 'Anonymous',
	movie int not null foreign key references Movie(ID) on delete cascade,
)

select  * from Movie m 
inner join Actor_Movie a on (a.movie = m.ID)
inner join Person p1 on (p1.ID = a.person)
inner join Director_Movie d on (d.movie = m.ID)
inner join Genre_Movie gm on (gm.movie = m.ID)
inner join Genre g on (g.ID = gm.genre)
inner join Language_Movie lm on (lm.movie = m.ID)
inner join [Language] l on (l.ID = lm.[language])
inner join Country_Movie cm on (cm.movie = m.ID)
inner join Country c on (c.ID = cm.country)
inner join Review r on (r.movie = m.ID)
where m.id = 1

go
create procedure mp_movies_by_genre @genre varchar(32)
as begin
	select m.ID from Movie m
	inner join Genre_Movie gm on (gm.movie = m.ID)
	inner join Genre g on (gm.genre = g.ID)
	where g.title = @genre;
end

go
create procedure mp_movie_info_array @movie_id int, @array_name varchar(32)
as begin
	if @array_name = 'actor'
	begin
		select * from person p
		inner join Actor_Movie am on (am.person = p.ID)
		inner join Movie m on (am.movie = m.ID)
		where m.ID = @movie_id
	end
	if @array_name = 'director'
	begin
		select * from person p
		inner join Director_Movie dm on (dm.person = p.ID)
		inner join Movie m on (dm.movie = m.ID)
		where m.ID = @movie_id
	end
	if @array_name = 'genre'
	begin
		select * from genre g
		inner join Genre_Movie gm on (gm.genre = g.ID)
		inner join Movie m on (gm.movie = m.ID)
		where m.ID = @movie_id
	end
	if @array_name = 'language'
	begin
		select * from [Language] l
		inner join Language_Movie lm on (lm.[language] = l.ID)
		inner join Movie m on (lm.movie = m.ID)
		where m.ID = @movie_id
	end
	if @array_name = 'country'
	begin
		select * from Country c
		inner join Country_Movie cm on (cm.country = c.ID)
		inner join Movie m on (cm.movie = m.ID)
		where m.ID = @movie_id
	end
	if @array_name = 'review'
	begin
		select * from Review r
		where r.movie = @movie_id
	end
	else
	begin
		print 'not supported'
	end
end
