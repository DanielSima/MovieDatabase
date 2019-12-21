create table Movie (
	ID int primary key identity(1,1),
	title nvarchar(64) not null,
	[description] nvarchar(512) not null,
	release_date date not null,
	runtime int not null check(runtime > 0),
	rating decimal(2,1) not null check(rating >= 0.0 and rating <= 10.0),
	poster_path varchar(128),
	budget int check(budget > 0),
)

create table Person (
	ID int primary key identity(1,1),
	name nvarchar(64) not null,
	date_of_birth date not null check(date_of_birth < getdate()),
	place_of_birth varchar(32),
	gender int not null check(gender >= 0 and gender <= 2),
	photo_path varchar(128),
)

create table Actor_Movie (
	ID int primary key identity(1,1),
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
	title varchar(32) not null,
)

create table Genre_Movie (
	ID int primary key identity(1,1),
	genre int not null foreign key references Genre(ID) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table [Language] (
	iso_code varchar(2) primary key,
	name varchar(32) not null,
)

create table Language_Movie (
	ID int primary key identity(1,1),
	[language] varchar(2) not null foreign key references [Language](iso_code) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Country (
	iso_code varchar(2) primary key,
	name varchar(32) not null,
)

create table Country_Movie (
	ID int primary key identity(1,1),
	country varchar(2) not null foreign key references Country(iso_code) on delete cascade,
	movie int not null foreign key references Movie(ID) on delete cascade,
)

create table Review (
	ID int primary key identity(1,1),
	[description] nvarchar(512) not null,
	date_created date not null default getdate(),
	author nvarchar(32) not null default 'Anonymous',
	movie int not null foreign key references Movie(ID) on delete cascade,
)

insert into Movie(title, [description], release_date, runtime, rating, poster_path, budget) values (
'Fight Club', 
'A ticking-time-bomb insomniac and a slippery soap salesman channel primal male aggression into a shocking new form of therapy. Their concept catches on, with underground \"fight clubs\" forming in every town, until an eccentric gets in the way and ignites an out-of-control spiral toward oblivion.',
'1999-10-12',
139,
7.8,
'/7PzJdsLGlR7oW4J0J5Xcd0pHGRg.png',
63000000);

insert into Person(name, date_of_birth, place_of_birth, gender, photo_path) values (
'Brad Pitt',
'1963-12-18',
'Shawnee, Oklahoma, USA',
2,
'/kU3B75TyRiCgE270EyZnHjfivoq.jpg');

insert into Actor_Movie values(1,1);

insert into Director_Movie values(1,1);

insert into Genre(title) values('Action');

insert into Genre_Movie values(1,1);

insert into [Language](iso_code, name) values ('en', 'English');
insert into [Language](iso_code, name) values ('cs', 'Czech');

insert into Language_Movie values('en',1);
insert into Language_Movie values('cs',1);

insert into Country(iso_code, name) values ('US', 'United States of America');

insert into Country_Movie values('US',1);

insert into Review([description],author, movie) values (
'Like most of the reviews here, I agree that Guardians of the Galaxy was an absolute hoot. Guardians never takes itself too seriously which makes this movie a whole lot of fun.\r\n\r\nThe cast was perfectly chosen and even though two of the main five were CG, knowing who voiced and acted alongside them completely filled out these characters.\r\n\r\nGuardians of the Galaxy is one of those rare complete audience pleasers. Good fun for everyone!',
'Travis Bell',
1);
insert into Review([description], movie) values (
'2he reviews here, I agree that Guardians of the Galaxy was an absolute hoot. Guardians never takes itself too seriously which makes this movie a whole lot of fun.\r\n\r\nThe cast was perfectly chosen and even though two of the main five were CG, knowing who voiced and acted alongside them completely filled out these characters.\r\n\r\nGuardians of the Galaxy is one of those rare complete audience pleasers. Good fun for everyone!',
1);


select  * from Movie m
delete from movie;
inner join Actor_Movie a on (a.movie = m.ID)
inner join Person p1 on (p1.ID = a.person)
inner join Director_Movie d on (d.movie = m.ID)
inner join Genre_Movie gm on (gm.movie = m.ID)
inner join Genre g on (g.ID = gm.genre)
inner join Language_Movie lm on (lm.movie = m.ID)
inner join [Language] l on (l.iso_code = lm.[language])
inner join Country_Movie cm on (cm.movie = m.ID)
inner join Country c on (c.iso_code = cm.country)
inner join Review r on (r.movie = m.ID)