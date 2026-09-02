TRUNCATE TABLE Assets
INSERT INTO Assets VALUES 
	('grass', 'Grass', '1'),
	('desert', 'Desert', '1'),
	('water', 'Ocean', '1'),
	('rocky', 'Rocky', '1'),
	('swamp', 'Swamp', '1'),
	('unknown', 'Unknown', '1'),
	('fogowar', 'Fog of War', '1'),
	('mountains', 'Mountains', '0.8'),
	('hills', 'Hills', '0.8'),
	('forest', 'Forest', '0.8'),
	('buildings', 'Outpost', '0.8'),
	('civilisation', 'Civilisation', '0.8'),
	('flowers', 'Flowers', '0.8'),
	('abode', 'Abode', '0.8'),
	('cave', 'Cave', '0.8'),
	('ruin', 'Ruin', '0.8');

TRUNCATE TABLE USERS;
INSERT INTO USERS VALUES
	-- (USERNAME, ISADMIN, GOLD, BASTIONTURNS, HASBASTION)
	('mole', 1, 1000, 0, 0),
	('halo', 0, 0, 0, 0),
	('viridia', 0, 0, 0, 0),
	('sont', 0, 0, 0, 0),
	('vilmora', 0, 0, 0, 0),
	('fedwir', 0, 0, 0, 0),
	('krog urtle', 0, 0, 0, 0),
	('rob', 0, 0, 0, 1),
	('hestegor', 0, 0, 0, 0),
	('vesle', 0, 0, 0, 0),
	('evelyn', 0, 0, 0, 0),
	('ursula', 0, 0, 0, 0),
	('laralei', 0, 0, 0, 0),
	('botrytus', 0, 0, 0, 0),
	('chryseehk', 0, 0, 0, 0),
	('nana', 0, 0, 0, 0),
	('guest', 0, 0, 0, 0),
	('freya', 0, 0, 0, 0);

