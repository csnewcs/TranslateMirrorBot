create database if not exists translatemirrorbot;
create user if not exists translatemirrorbot@localhost;
create schema if not exists translatemirrorbot;
grant all privileges on translatemirrorbot.* to translatemirrorbot@localhost;