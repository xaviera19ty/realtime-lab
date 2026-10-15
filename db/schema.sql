CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;
CREATE TABLE `users` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `email` varchar(255) NOT NULL,
    `password_hash` varchar(100) NULL,
    `name` varchar(50) NOT NULL,
    `role` varchar(20) NOT NULL DEFAULT 'user',
    `is_guest` tinyint(1) NOT NULL,
    `is_demo` tinyint(1) NOT NULL,
    `created_at` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE UNIQUE INDEX `IX_users_email` ON `users` (`email`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261015134647_InitialUsers', '10.0.9');

COMMIT;

