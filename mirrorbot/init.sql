create database if not exists translatemirrorbot;
create user if not exists translatemirrorbot@localhost;
create schema if not exists translatemirrorbot;
grant all privileges on translatemirrorbot.* to translatemirrorbot@localhost;
use translatemirrorbot
create table NoticeGuilds (GuildID bigint unsigned, ChannelID bigint unsigned);
create table Used (day DATE, papago INT, kakao INT);