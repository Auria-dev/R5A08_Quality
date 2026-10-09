
INSERT INTO t_e_marque (mrq_nom) VALUES ('Apple'), ('Samsung'), ('Google'), ('Xiaomi'), ('OnePlus');
INSERT INTO t_e_typeproduit (tpp_nom) VALUES ('Smartphone'), ('Tablette'), ('Montre connectée'), ('Écouteurs');

INSERT INTO t_e_produit (prd_nom, prd_description, prd_nomphoto, prd_uriphoto, prd_idtype, prd_idmarque, prd_stockreel, prd_stockmin, prd_stockmax)
VALUES
(
    'iPhone 15',
    'Smartphone Apple iPhone 15',
    'iphone15.jpg',
    '/images/iphone15.jpg',
    (SELECT tpp_id FROM t_e_typeproduit WHERE tpp_nom = 'Smartphone'),
    (SELECT mrq_id FROM t_e_marque WHERE mrq_nom = 'Apple'),
    25,
    5,
    50
),
(
    'Galaxy S24',
    'Smartphone Samsung Galaxy S24',
    'galaxy-s24.jpg',
    '/images/galaxy-s24.jpg',
    (SELECT tpp_id FROM t_e_typeproduit WHERE tpp_nom = 'Smartphone'),
    (SELECT mrq_id FROM t_e_marque WHERE mrq_nom = 'Samsung'),
    20,
    5,
    40
),
(
    'Pixel 9',
    'Smartphone Google Pixel 9',
    'pixel9.jpg',
    '/images/pixel9.jpg',
    (SELECT tpp_id FROM t_e_typeproduit WHERE tpp_nom = 'Smartphone'),
    (SELECT mrq_id FROM t_e_marque WHERE mrq_nom = 'Google'),
    15,
    5,
    30
),
(
    'Xiaomi 14',
    'Smartphone Xiaomi 14',
    'xiaomi14.jpg',
    '/images/xiaomi14.jpg',
    (SELECT tpp_id FROM t_e_typeproduit WHERE tpp_nom = 'Smartphone'),
    (SELECT mrq_id FROM t_e_marque WHERE mrq_nom = 'Xiaomi'),
    30,
    5,
    60
),
(
    'OnePlus 12',
    'Smartphone OnePlus 12',
    'oneplus12.jpg',
    '/images/oneplus12.jpg',
    (SELECT tpp_id FROM t_e_typeproduit WHERE tpp_nom = 'Smartphone'),
    (SELECT mrq_id FROM t_e_marque WHERE mrq_nom = 'OnePlus'),
    18,
    5,
    40
);