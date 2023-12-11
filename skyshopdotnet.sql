create database skyshopdotnet
go
use skyshopdotnet
go
--
--use master
--drop database skyshopdotnet

create table [Role]
(
	id_role int identity(1,1) primary key,
	role_name varchar(30)
)

create table [User]
(
	id_user int identity(1,1) primary key,
	username nvarchar(50) unique,
	email varchar(40) CHECK(email LIKE '%@%') unique,
	phone varchar(10) CHECK(phone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') unique,
	[password] varchar(255),
	avatar_user text,
	id_role int foreign key references [Role](id_role)
	on delete cascade
	on update cascade
)

create table [Category]
(
	id_cate int identity(1,1) primary key,
	cate_name nvarchar(max)
)
create table Brand (
	id_brand int identity(1,1) primary key,
	brand_name varchar(20),
)
create table [Product]
(
	id_product int identity(1,1) primary key,
	product_name nvarchar(max),
	price decimal check (price >= 0),
	[desc] nvarchar(max),
	id_cate int foreign key references Category(id_cate)
	on delete cascade
	on update cascade,
	id_brand int foreign key references Brand(id_brand)
)
create table [Product_Image]
(
	id_image int identity(1,1) primary key,
	[image] text,
	id_product int foreign key references Product(id_product)
	on delete cascade
	on update cascade
)

create table Size(
	Size_name varchar(5) primary key,
)
create table DetailSizePd (
	idDetailSPd int identity(1,1) primary key,
	id_product int foreign key references Product(id_product),
	Size_name varchar(5) foreign key references Size(Size_name)
)


create table Evaluation
(
	id_cust int references [User](id_user)
	on delete cascade
	on update cascade,
	id_product int references Product(id_product)
	on delete cascade
	on update cascade,
	content nvarchar(max),
	comment_date date,
	[image] text,
	primary key(id_cust, id_product)
)
create table [Order]
(
	id_order int identity(1,1) primary key,
	id_cust int foreign key references [User](id_user)
	on delete cascade
	on update cascade,
	[name] nvarchar(50),
	date_create date,
	[address] nvarchar(max),
	phone varchar(10),
	email varchar(30) CHECK(email LIKE '%@%'),
	note nvarchar(max),
	payment_status nvarchar(30)
)
CREATE TABLE [Order_Detail]
(
	id_product int REFERENCES Product(id_product)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	id_order int REFERENCES [Order](id_order)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	quantity int CHECK (quantity > 0),
	size char(3),
	total decimal CHECK (total >= 0),
	PRIMARY KEY (id_product, id_order)
)


 --Thêm dữ liệu cho bảng [Role]
insert into [Role] (role_name)
		values
		('Admin'),
		('User')

-- Thêm dữ liệu cho bảng [User]
INSERT INTO [User] (username, email, phone, [password], id_role)
VALUES
    ('admin', 'admin@example.com', '1234567890', 'admin123', 1),
    ('user1', 'user1@example.com', '9876543210', 'user123', 2),
    ('user2', 'user2@example.com', '5555555555', 'user456', 2);
-- Thêm dữ liệu cho bảng [Category]
INSERT INTO [Category] (cate_name)
VALUES
    (N'Áo'),
    (N'Quần'),
    (N'Giày'),
	(N'Phụ kiện');
INSERT INTO [Size] (Size_name)
VALUES
    ('S'),
	('L'),
	('M'),
	('XS'),
	('XL'),
	('XXL'),
	('39'),
	('41'),
	('42'),
	('37'),
	('38');

	Insert into Brand (Brand_name)
	values 
	('Adidas'),
	('Converse'),
	('Hades'),
	('Lata'),
	('Kilyeyewear'),
	('New Balance'),
	('Nike'),
	('SSStutter'),
	('Tepys Accessories'),
	('Vans')

INSERT INTO [Product] (product_name, price, [desc], id_brand,id_cate)
VALUES
	--Áo
    (N'Áo Phông - Áo thun Tập Luyện Nam Adidas', 665000, N'Kiểu dáng ôm sát mà co giãn cho phạm vi chuyển động tối đa và loại bỏ mồ hôi trên da trong khi tập luyện. Logo graphic táo bạo ở mặt trước khẳng định tinh thần tập luyện của bạn. Sản phẩm này may bằng vải công nghệ Primegreen, thuộc dòng chất liệu tái chế hiệu năng cao', 1, 1),
    (N'Áo Phông - Áo thun Chạy Nam NIKE As M Nk Df Miler', 409500, N'Dáng standard fit phù hợp tiêu chuẩn. Cổ thuyền viên với cổ áo có gân. Tay áo ngắn. Hình ảnh Jordan ở phía trước', 7, 1),
    (N'Áo thun Vans Checkered Side Stripe SS Tee', 720000, N'Chất liệu Cotton cao cấp giúp vận động thoải mái, tone màu trắng đơn giản dễ phối đồ. Đặc biệt là họa tiết logo Vans được cách điệu, đồng bộ ở ngực trái và sau lưng giúp bạn có được một chiếc áo vô cùng cá tính và sành điệu.', 10, 1),
	(N'Áo Phông - Áo thun Chạy Nữ ADIDAS Own The Run', 637500, N'Áo Phông - Áo thun Originals Nam ADIDAS Trefoil Tee HC7118. 100% chính hãng ADIDAS Việt Nam. Bao gồm: Sản phẩm mới nguyên TAG, hóa đơn bán hàng từ SALEHUB', 1, 1),
	(N'Men New Balance Sports Style Optiks Short Sleeve', 595000, N'Áo thun Essentials Stacked Logo dành cho nam được thiết kế vừa vặn, ôm sát ngực, eo và hông, giúp bạn điều chỉnh chuyển động mà không gây cảm giác bó sát, cồng kềnh hay gò bó. Tay áo ngắn mang đến khả năng thoáng khí đặc biệt trong những ngày ấm áp hơn, đồng thời vải cotton dệt kim jersey mềm mại và nhẹ nhàng ôm sát vào làn da của bạn.', 6, 1),
	(N'Women New Balance Athletics Higher Learning Slim Tee',348000, N'NB Athletics Higher Learning Slim Tee là một món đồ bắt buộc phải có để dễ dàng chuyển đổi từ mùa này sang mùa khác. Chiếc áo thun nữ này mang phong cách preppy và các chi tiết lấy cảm hứng từ trường đại học để mang đến cho bạn vẻ ngoài táo bạo và biểu cảm cho các hoạt động thường ngày, thường ngày của bạn.', 6, 1),
	(N'Áo thun Converse Mountain Moon Tee GRAPHICS-SS SEASONA',600000 , N'Thiết kế với form dáng áo thun cơ bản dễ mặc dễ mix-match. Nổi bật nhất vẫn là graphic dạng tròn với dòng chữ "Converse Mountain Club" siêu chất mang đến cho các tín đồ thời trang một items đậm chất street style.',2, 1),
	(N'Áo Thun Nam Dài Tay SSSTUTTER cổ tròn chất liệu cotton',149500 , N'Được làm từ chất liệu nỉ thấm hút mồ hôi kết hợp cùng form áo vừa vặn không quá rộng giúp cho người mặc thoải mái hoạt động, giảm thiểu tối đa tình trạng mồ hôi thấm ngược. Một chiếc áo vừa Tinh Tế lại vô cùng dễ chịu như vậy thì chắc chắn không chàng trai nào muốn bỏ lỡ đâu.', 8, 1),
	(N'Áo croptop nữ SSSTUTTER cổ cao nhún bèo chất cotton thoáng mát Vase Top',249000 , N'Vase Top được SSS. lựa chọn dáng áo Croptop ngắn tay Năng Động nhưng cũng không kém phần Nữ Tính với thiết kế vạt bèo nhấn ở tay và cổ áo. Kết hợp cùng loại vải Cotton mềm mát, chắc chắn Vase Top sẽ làm siêu lòng cả các nàng khó tính nhất.', 8, 1),
	(N'Áo thun Bóng Rổ Nam Nike M J Jumpman',573300 , N'Dáng standard fit phù hợp tiêu chuẩn. Cổ thuyền viên với cổ áo có gân. Tay áo ngắn. Hình ảnh Jordan ở phía trước', 7, 1),
	--Quần
	(N'New Balance Q Speed Fuel 2 in 1 5 inch Shorts', 995000, N'Để có chức năng tối ưu và thiết kế hợp thời trang, Q Speed ​​Fuel 2 in 1 5 Inch Short của chúng tôi sẽ đi được xa. Những chiếc quần short nam thoáng khí này được thiết kế dành cho người chạy bộ với các tính năng như NB Dry giúp hút ẩm và lớp lót nén tích hợp mang lại sự thoải mái nhẹ nhàng trong mỗi sải chân.', 6, 2),
	(N'Quần Dài Originals Nam Adidas B Trf Abs Swtpt', 900000, N'Kiểu dáng quần ôm vừa với cạp chun có dây rút. Quần có các túi mở rộng rãi ở 2 bên. Sử dụng chất liệu Vải thun da cá làm từ 100% cotton', 1, 2),
	(N'HADES GLOSSY WATER SHORTS',190000 , N'Chất liệu thoáng mát, dễ vận động. Quần lưng chun có dây rút. Chữ trên quần được in kéo lụa', 3, 2),
	(N'Quần âu ống đúng nam SSSTUTTER đỉa quần bản to cách điệu DORE TROUSERS',449000 , N' Sản phẩm đúng với hình ảnh, thậm chí đẹp hơn trên hình, cả nam và nữ mặc lên đều phù hợp. Bao bì nylon trắng đục có thể thấy được sản phẩm bên trong, tái chế chống mưa và hạn chế các tác động vật lý', 8, 2),
	(N'Quần âu nữ SSSTUTTER ống suông BIRDIE PANTS',392000 , N'Màu: đen, xám nhạt, xám đậm. Size: 0/1/2 tương ứng với S/M/L. Chất liệu: typsi. Form dáng: cullotes ống suông', 8, 2),
	(N'ENERGETIC PANT', 650000, N'Chất liệu thoáng mát, dễ vận động. Quần lưng chun có dây rút.', 3, 2),
	(N'Men New Balance Athletics Higher Learning Fleece Shorts',648000 , N'Lấy cảm hứng từ giới học thuật, chiếc áo khoác ngắn lông cừu dành cho học tập cao hơn của môn điền kinh NB sẽ mang lại vẻ cổ điển cho tủ quần áo thông thường của bạn. Lông cừu terry Pháp ấm áp, ấm áp giúp bạn luôn thoải mái, đồng thời các túi dễ lấy giúp bạn đựng những món đồ nhỏ trong tầm tay.', 6, 2),
	--Giày
	(N'Giày sneakers Vans UA Old Skool Packing Tape', 710000, N'Vans đã ứng dụng họa tiết logo chắp vá đầy cuốn hút, bao phủ lên thân giày, tạo một vẻ ngoài bắt mắt. Thân giày mang màu trắng ngà đậm chất vintage với công nghệ in màu hiện đại giúp giày không gặp phải tình trạng bạc màu hay lem màu khi sử dụng', 10, 3),
	(N'Giày Thể Thao Unisex ADIDAS X9000L4 ',1990000 , N'Giày Thể Thao Unisex ADIDAS X9000L4 U HR1727. 100% chính hãng ADIDAS Việt Nam. Bao gồm: Sản phẩm mới nguyên TAG, hóa đơn bán hàng từ SALEHUB', 1, 3),
	(N'Men New Balance 57/40 Classic Sneakers',2890000 , N'Men New Balance 57/40 Classic Sneakers', 6, 3),
	(N'Giày sneakers Converse Chuck Taylor All Star Renew',1600000 , N'Cá tính và đậm chất hơn cùng cách phối màu cực cool ngầu trong phiên bản Converse Chuck Taylor All Star Renew. Được làm từ tổ hợp chất liệu tái chế nhưng vẫn thật bền bỉ và cuốn hút, BST giúp nhà Converse truyền đi thông điệp về một lối sống xanh, giảm thiểu rác thải để tương lai chúng ta được tốt đẹp hơn.', 2, 3),
	(N'Giày sneakers Converse Chuck Taylor All Star 1970s',2900000 , N'Converse Chuck 70 Recycled RPET Canvas được “tái sinh” với chất liệu tái chế bảo vệ môi trường tối đa. Bên cạnh đó, phối màu hồng ngọt ngào sẽ giúp bạn có thêm nguồn năng lượng tích cực cho mọi outfit của mình. Với những ai trót mê Converse thì đây sẽ là “must have item” mà bạn không nên bỏ lỡ.', 2, 3),
	(N'Giày Thể Thao Nam NIKE Air Max Dawn DM0013-101', 1519000, N'Giày Thể Thao Nam NIKE Air Max Dawn DM0013-101. 100% chính hãng NIKE Việt Nam. Bao gồm: Sản phẩm mới nguyên TAG, hóa đơn bán hàng từ SALEHUB', 7, 3),
	(N'Giày Nam Nike Benassi Slp 882410-003', 1642600, N'Giày Nam Nike Benassi Slp 882410-003. 100% chính hãng Nike Việt Nam. Bao gồm: Sản phẩm, hộp giày, hóa đơn bán hàng từ Salehub', 7, 3),
	--Phụ kiện
	(N'Gọng kính cận nam nữ Lilyeyewear chất liệu kim loại thanh mảnh nhẹ nhàng',179000 ,N' Kính mắt Lily cam kết 100% sản phẩm là ảnh thật shop tự chụp, quý khách hoàn toàn yên tâm khi mua và sử dụng sản phẩm. Kính mắt Lily nghiêm cấm mọi hành vi sao chép, vi phạm bản quyền hình ảnh.',5,4),
	(N'SJB Steed',210000 ,N'Kết cấu SJB như những chiếc răng cá mập sắc nhọn lao đến, giữ chặt lấy mục tiêu, mang những tính chất của một người nhạy bén, tinh tế chứng minh qua sự hài lòng của mọi tín đồ thời trang Việt',6,4),
	(N'Bông tai GreenLucky nam nữ bạc 925 không kích ứng',245000 ,N'Chất liệu Mạ bạc 925. Khó phai màu. Khó rỉ sét. Để sản phẩm sáng và bóng. bạn nên đeo tránh tiếp xúc với các hoá chất tẩy rửa mạnh.',9,4),
	(N'Khuyên vành tai Titan nữ tạo hình độc đáo',187000 ,N'Chất liệu Mạ bạc 925. Khó phai màu. Khó rỉ sét. Để sản phẩm sáng và bóng. bạn nên đeo tránh tiếp xúc với các hoá chất tẩy rửa mạnh.',9,4),
	(N'Túi xách nữ cao cấp Lata TX09',399000 ,N'Túi xách nữ Lata TX09 được làm từ chất liệu da tổng hợp cao cấp với thiết kế đơn giản, sang trọng. Form túi chuẩn đẹp đến từng đường kim mũi chỉ, chắc chắn sẽ làm bạn hài lòng. Kích thước túi rộng rãi, thoải mái, giúp bạn có thể đựng được nhiều đồ dùng cần thiết.',4,4),
	(N'Ví cầm tay nữ nắp gập dáng dài Lata VN71',299000 ,N'Ví cầm tay nữ nắp gập dáng dài Lata VN71 được thiết kế kiểu nắp gập cách điệu thời trang, đường sơn thủ công tỉ mỉ, đường viền chỉ chắc chắn, logo sắc nét.',4,4),
	(N'Ba lô nam nữ SSSTUTTER form cơ bản có túi chống sốc ngăn ngoài vật dụng', 244300,N'Ba lô nam nữ SSSTUTTER form cơ bản có túi chống sốc ngăn ngoài vật dụng BLANK BALO',8,4);

-- Thêm dữ liệu cho bảng [Product_Image]
INSERT INTO [Product_Image] ([image], id_product)
VALUES
	--Áo
    ('dosiin-adidas-ao-phong-ao-thun-tap-luyen-nam-adidas-tbos-tee-hl-525897525897.jpg', 1),
    ('dosiin-adidas-ao-phong-ao-thun-tap-luyen-nam-adidas-tbos-tee-hl-525898525898.jpg', 1),
    ('dosiin-adidas-ao-phong-ao-thun-tap-luyen-nam-adidas-tbos-tee-hl-525899525899.jpg', 1),
	('dosiin-nike-ao-phong-ao-thun-chay-nam-nike-as-m-nk-df-rdvn-ris-fls-gx-dd-526577526577.jpg',2),
	('dosiin-nike-ao-phong-ao-thun-chay-nam-nike-as-m-nk-df-rdvn-ris-fls-gx-dd-526578526578.jpg',2),
	('dosiin-nike-ao-phong-ao-thun-chay-nam-nike-as-m-nk-df-rdvn-ris-fls-gx-dd-526580526580.jpg',2),
	('dosiin-vans-ao-thun-vans-checkered-side-stripe-ss-tee-vn-alkz-329705329705.jpg',3),
	('dosiin-vans-ao-thun-vans-checkered-side-stripe-ss-tee-vn-alkz-329706329706.jpg',3),
	('dosiin-vans-ao-thun-vans-checkered-side-stripe-ss-tee-vn-alkz-329707329707.jpg',3),
	('dosiin-adidas-ao-phong-ao-thun-chay-nu-adidas-own-the-run-tee-gj-207828207828.jpg',4),
	('dosiin-adidas-ao-phong-ao-thun-chay-nu-adidas-own-the-run-tee-gj-207829207829.jpg',4),
	('dosiin-adidas-ao-phong-ao-thun-chay-nu-adidas-own-the-run-tee-gj-207831207831.jpg',4),
	('dosiin-new-balance-men-s-new-balance-sports-style-optiks-shortsleeve-tee-168931168931.jpg',5),
	('dosiin-new-balance-men-s-new-balance-sports-style-optiks-shortsleeve-tee-168932168932.jpg',5),
	('dosiin-new-balance-men-s-new-balance-sports-style-optiks-shortsleeve-tee-168934168934.jpg',5),
	('dosiin-new-balance-women-s-new-balance-athletics-higher-learning-slim-tee-390611390611.jpg',6),
	('dosiin-new-balance-women-s-new-balance-athletics-higher-learning-slim-tee-390612390612.jpg',6),
	('dosiin-new-balance-women-s-new-balance-athletics-higher-learning-slim-tee-390613390613.jpg',6),
	('dosiin-converse-ao-thun-converse-mountain-moon-tee-graphics-ss-seasonal-t-east-village-327584327584.jpg',7),
	('dosiin-converse-ao-thun-converse-mountain-moon-tee-graphics-ss-seasonal-t-east-village-327585327585.jpg',7),
	('dosiin-converse-ao-thun-converse-mountain-moon-tee-graphics-ss-seasonal-t-east-village-327586327586.jpg',7),
	('dosiin-ssstutter-ao-thun-nam-dai-tay-ssstutter-co-tron-chat-lieu-cotton-thoang-mat-kind-sweatshi353496.jpg',8),
	('dosiin-ssstutter-ao-thun-nam-dai-tay-ssstutter-co-tron-chat-lieu-cotton-thoang-mat-kind-sweatshi353491.jpg',8),
	('dosiin-ssstutter-ao-thun-nam-dai-tay-ssstutter-co-tron-chat-lieu-cotton-thoang-mat-kind-sweatshi353493.jpg',8),
	('dosiin-ssstutter-ao-croptop-nu-ssstutter-co-cao-nhun-beo-chat-cotton-thoang-mat-vase-top-353555353555.jpg',9),
	('dosiin-ssstutter-ao-croptop-nu-ssstutter-co-cao-nhun-beo-chat-cotton-thoang-mat-vase-top-353556353556.jpg',9),
	('dosiin-ssstutter-ao-croptop-nu-ssstutter-co-cao-nhun-beo-chat-cotton-thoang-mat-vase-top-353557353557.jpg',9),
	('dosiin-nike-ao-phong-ao-thun-bong-ro-nam-nike-m-j-jumpman-air-hbr-ss-crew-cv-216583216583.jpg',10),
	('dosiin-nike-ao-phong-ao-thun-bong-ro-nam-nike-m-j-jumpman-air-hbr-ss-crew-cv-216584216584.jpg',10),
	('dosiin-nike-ao-phong-ao-thun-bong-ro-nam-nike-m-j-jumpman-air-hbr-ss-crew-cv-216585216585.jpg',10),
	-- Quần
	('dosiin-new-balance-men-s-new-balance-q-speed-fuel-in-inch-shorts-520671520671.jpg',11),
	('dosiin-new-balance-men-s-new-balance-q-speed-fuel-in-inch-shorts-520672520672.jpg',11),
	('dosiin-new-balance-men-s-new-balance-q-speed-fuel-in-inch-shorts-520673520673.jpg',11),
	('dosiin-adidas-quan-dai-originals-nam-adidas-b-trf-abs-swtpt-ge-206722206722.jpg',12),
	('dosiin-adidas-quan-dai-originals-nam-adidas-b-trf-abs-swtpt-ge-206723206723.jpeg',12),
	('dosiin-adidas-quan-dai-originals-nam-adidas-b-trf-abs-swtpt-ge-206725206725.jpeg',12),
	('dosiin-hades-hades-glossy-water-shorts-362383362383.jpg',13),
	('dosiin-hades-hades-glossy-water-shorts-362384362384.jpg',13),
	('dosiin-hades-hades-glossy-water-shorts-362385362385.jpg',13),
	('dosiin-ssstutter-quan-au-ong-dung-nam-ssstutter-dia-quan-ban-to-cach-dieu-dore-trousers-497735497735.jpg',14),
	('dosiin-ssstutter-quan-au-ong-dung-nam-ssstutter-dia-quan-ban-to-cach-dieu-dore-trousers-497737497737.jpg',14),
	('dosiin-ssstutter-quan-au-ong-dung-nam-ssstutter-dia-quan-ban-to-cach-dieu-dore-trousers-497738497738.jpg',14),
	('dosiin-ssstutter-quan-au-nu-ssstutter-ong-suong-birdie-pants-497662497662.jpg',15),
	('dosiin-ssstutter-quan-au-nu-ssstutter-ong-suong-birdie-pants-497663497663.jpg',15),
	('dosiin-ssstutter-quan-au-nu-ssstutter-ong-suong-birdie-pants-497664497664.jpg',15),
	('dosiin-hades-energetic-pant-533373533373.jpg',16),
	('dosiin-hades-energetic-pant-533375533375.jpg',16),
	('dosiin-hades-energetic-pant-533377533377.jpg',16),
	('dosiin-new-balance-men-s-new-balance-athletics-higher-learning-fleece-shorts-390707390707.jpg',17),
	('dosiin-new-balance-men-s-new-balance-athletics-higher-learning-fleece-shorts-390708390708.jpg',17),
	('dosiin-new-balance-men-s-new-balance-athletics-higher-learning-fleece-shorts-390710390710.jpg',17),
	--Giày
	('dosiin-vans-giay-sneakers-vans-ua-old-skool-packing-tape-vn-a-u-bwn-329509329509.jpg',18),
	('dosiin-vans-giay-sneakers-vans-ua-old-skool-packing-tape-vn-a-u-bwn-329510329510.jpg',18),
	('dosiin-vans-giay-sneakers-vans-ua-old-skool-packing-tape-vn-a-u-bwn-329511329511.jpg',18),
	('dosiin-adidas-giay-the-thao-unisex-adidas-xlu-hr-533745533745.jpg',19),
	('dosiin-adidas-giay-the-thao-unisex-adidas-xlu-hr-533746533746.jpg',19),
	('dosiin-adidas-giay-the-thao-unisex-adidas-xlu-hr-533747533747.jpg',19),
	('dosiin-new-balance-men-s-new-balance-classic-sneakers-521296521296.jpg',20),
	('dosiin-new-balance-men-s-new-balance-classic-sneakers-521299521299.jpg',20),
	('dosiin-new-balance-men-s-new-balance-classic-sneakers-521301521301.jpg',20),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-renew-v-327382327382.jpg',21),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-renew-v-327383327383.jpg',21),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-renew-v-327384327384.jpg',21),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-s-recycled-rpet-canvas-c-391789391789.jpg',22),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-s-recycled-rpet-canvas-c-391786391786.jpg',22),
	('dosiin-converse-giay-sneakers-converse-chuck-taylor-all-star-s-recycled-rpet-canvas-c-391790391790.jpg',22),
	('dosiin-nike-giay-the-thao-nam-nike-air-max-dawn-dm-533820533820.jpg',23),
	('dosiin-nike-giay-the-thao-nam-nike-air-max-dawn-dm-533821533821.jpg',23),
	('dosiin-nike-giay-the-thao-nam-nike-air-max-dawn-dm-533822533822.jpg',23),
	('dosiin-nike-giay-nam-nike-benassi-slp-147327147327.jpeg',24),
	('dosiin-nike-giay-nam-nike-benassi-slp-147328147328.jpg',24),
	('dosiin-nike-giay-nam-nike-benassi-slp-147329147329.jpg',24),
	--Phụ kiện
	('dosiin-lilyeyewear-gong-kinh-can-nam-nu-lilyeyewear-chat-lieu-kim-loai-thanh-manh-nhe-nhang-4858485897.jpg',25),
	('dosiin-lilyeyewear-gong-kinh-can-nam-nu-lilyeyewear-chat-lieu-kim-loai-thanh-manh-nhe-nhang-4858485899.jpg',25),
	('dosiin-lilyeyewear-gong-kinh-can-nam-nu-lilyeyewear-chat-lieu-kim-loai-thanh-manh-nhe-nhang-4859485902.jpg',25),
	('dosiin-vong-tay-paracord-sjb-steed-403391403391.jpg',26),
	('dosiin-vong-tay-paracord-sjb-steed-403391403391.jpg',26),
	('dosiin-vong-tay-paracord-sjb-steed-403391403391.jpg',26),
	('dosiin-tepys-accessories-bong-tai-greenlucky-nam-nu-bac-khong-kich-ung-509056509056.jpg',27),
	('dosiin-tepys-accessories-bong-tai-greenlucky-nam-nu-bac-khong-kich-ung-509057509057.jpg',27),
	('dosiin-tepys-accessories-bong-tai-greenlucky-nam-nu-bac-khong-kich-ung-509058509058.jpg',27),
	('dosiin-tepys-accessories-khuyen-vanh-tai-titan-nu-tao-hinh-doc-dao-506037506037.jpg',28),
	('dosiin-tepys-accessories-khuyen-vanh-tai-titan-nu-tao-hinh-doc-dao-506038506038.jpg',28),
	('dosiin-tepys-accessories-khuyen-vanh-tai-titan-nu-tao-hinh-doc-dao-506039506039.jpg',28),
	('dosiin-lata-tui-xach-nu-cao-cap-lata-tx-228532228532.jpg',29),
	('dosiin-lata-tui-xach-nu-cao-cap-lata-tx-228533228533.jpg',29),
	('dosiin-lata-tui-xach-nu-cao-cap-lata-tx-228534228534.jpg',29),
	('dosiin-lata-vi-cam-tay-nu-nap-gap-dang-dai-lata-vn-534317534317.jpg',30),
	('dosiin-lata-vi-cam-tay-nu-nap-gap-dang-dai-lata-vn-534319534319.jpg',30),
	('dosiin-lata-vi-cam-tay-nu-nap-gap-dang-dai-lata-vn-534320534320.jpg',30),
	('dosiin-ssstutter-ba-lo-nam-nu-ssstutter-form-co-ban-co-tui-chong-soc-ngan-ngoai-vat-dung-blank-b500501.jpg',31),
	('dosiin-ssstutter-ba-lo-nam-nu-ssstutter-form-co-ban-co-tui-chong-soc-ngan-ngoai-vat-dung-blank-b500502.jpg',31),
	('dosiin-ssstutter-ba-lo-nam-nu-ssstutter-form-co-ban-co-tui-chong-soc-ngan-ngoai-vat-dung-blank-b500503.jpg',31);

---- Thêm dữ liệu cho bảng Evaluation
--INSERT INTO Evaluation (id_cust, id_product, content, comment_date, [image])
--VALUES
--    (2, 1, 'Great T-shirt!', '2023-09-26', 'review_image1.jpg'),
--    (3, 1, 'Good quality dress', '2023-09-27', 'review_image2.jpg'),
--    (2, 2, 'Love the sunglasses', '2023-09-28', 'review_image3.jpg');
---- Thêm dữ liệu cho bảng [Order]
--INSERT INTO [Order] (id_cust, date_create, [address], phone, email, note, payment_status)
--VALUES
--    (2, '2023-09-26', '123 Main St', '9876543210', 'user1@example.com', 'Order for user1', 'Paid'),
--    (3, '2023-09-27', '456 Elm St', '5555555555', 'user2@example.com', 'Order for user2', 'Paid')
    
---- Thêm dữ liệu cho bảng [Order_Detail] 
--INSERT INTO [Order_Detail] (id_product, id_order, quantity, size, total)
--VALUES
--    (1, 1, 2, 'M', 39.98),
--    (2, 2, 1, 'L', 49.99);
------------------------------------------------------------------------------------------

go
select * from [Role]
select * from [User]
select * from Category
select * from Brand
select * from Product
select * from Product_Image
select * from [Size]
select * from [Order]
select * from Order_detail
 
go

--ALTER TABLE [User]
--ADD avatar_user text

--DELETE FROM [User] WHERE id_user = 5;
--select * from Product 
--Join Brand on Brand.id_brand= Product.id_brand

--select Product.*, Brand.brand_name from Product
--Join Brand on Brand.id_brand= Product.id_brand
--where Product.id_brand = 10

--select top 1 [image] from Product_Image
--where id_product = 1

--select p.id_product, p.product_name, p.price, p.[desc], p.id_brand, p.id_cate, 
--(SELECT TOP 1 [image] FROM Product_Image WHERE id_product = p.id_product) AS [image]
--from Product p;
--ALTER TABLE [Order]
--ALTER COLUMN note nvarchar(max);

--UPDATE [Order]
--SET note = N'áo k nhăn nha shop iu'
--WHERE id_order = 1;

--update [Order]
--set payment_status = N'Đã hoàn thành'
--where id_order = 4

--update [User]
--set [password]= '123'
--where id_user = 6

--select * from [User]

