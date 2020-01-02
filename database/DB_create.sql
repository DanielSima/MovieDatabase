create database Movie_Database;
use Movie_Database;

create login client  with password = 'password';  
go
create user client for login client;  
go 
grant select, insert, update, delete to client;
go

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

create index m_title on movie(title);
create index p_name on person(name);
create index a_person on actor_movie(person);
create index d_movie on director_movie(person);
create index g_title on genre(title);
create index gm_movie on genre_movie(movie);
create index r_author on review(author);
create index r_movie on review(movie);