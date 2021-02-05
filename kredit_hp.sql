-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 13, 2020 at 02:22 PM
-- Server version: 10.1.37-MariaDB
-- PHP Version: 7.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `kredit_hp`
--

-- --------------------------------------------------------

--
-- Table structure for table `tb_angsuran`
--

CREATE TABLE `tb_angsuran` (
  `kode_angsuran` varchar(5) NOT NULL,
  `kode_kredit` varchar(7) NOT NULL,
  `pembayaran_ke` int(5) NOT NULL,
  `tgl_bayar` varchar(20) NOT NULL,
  `bayar` int(15) NOT NULL,
  `keterangan` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `tb_handphone`
--

CREATE TABLE `tb_handphone` (
  `kode_handphone` varchar(5) NOT NULL,
  `merk` varchar(50) NOT NULL,
  `warna` varchar(20) NOT NULL,
  `tahun_launching` int(4) NOT NULL,
  `type` varchar(10) NOT NULL,
  `sistem_operasi` varchar(25) NOT NULL,
  `ram` varchar(5) NOT NULL,
  `memori_internal` varchar(5) NOT NULL,
  `harga` int(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tb_handphone`
--

INSERT INTO `tb_handphone` (`kode_handphone`, `merk`, `warna`, `tahun_launching`, `type`, `sistem_operasi`, `ram`, `memori_internal`, `harga`) VALUES
('HP001', 'Xiomi', 'Hitam Pekat', 2018, 'Redmi 6', 'Oreo', '3', '32', 1600000),
('HP002', 'Vivo', 'Emas', 2015, 'V5', 'Nuget', '2', '16', 1400000);

-- --------------------------------------------------------

--
-- Table structure for table `tb_kredit`
--

CREATE TABLE `tb_kredit` (
  `kode_kredit` varchar(7) NOT NULL,
  `mulai_kredit` varchar(20) NOT NULL,
  `kode_persyaratan` varchar(5) NOT NULL,
  `kode_handphone` varchar(5) NOT NULL,
  `harga_kredit` int(15) NOT NULL,
  `uang_muka` int(15) NOT NULL,
  `bunga` int(3) NOT NULL,
  `lama_cicilan` varchar(20) NOT NULL,
  `angsuran` int(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tb_kredit`
--

INSERT INTO `tb_kredit` (`kode_kredit`, `mulai_kredit`, `kode_persyaratan`, `kode_handphone`, `harga_kredit`, `uang_muka`, `bunga`, `lama_cicilan`, `angsuran`) VALUES
('KR001', '13-July-2020', 'SY002', 'HP002', 1610000, 161000, 15, '11', 131727);

-- --------------------------------------------------------

--
-- Table structure for table `tb_pelanggan`
--

CREATE TABLE `tb_pelanggan` (
  `kode_pelanggan` varchar(5) NOT NULL,
  `nama_pelanggan` varchar(255) NOT NULL,
  `jekel` enum('Laki-Laki','Perempuan') NOT NULL,
  `alamat` varchar(255) NOT NULL,
  `no_telepon` varchar(15) NOT NULL,
  `pekerjaan` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tb_pelanggan`
--

INSERT INTO `tb_pelanggan` (`kode_pelanggan`, `nama_pelanggan`, `jekel`, `alamat`, `no_telepon`, `pekerjaan`) VALUES
('PL001', 'Reza', 'Perempuan', 'Makasar', '08121474831', 'Dokter'),
('PL002', 'Ning', 'Laki-Laki', 'Palu', '08123412311', 'Mahasiswa');

-- --------------------------------------------------------

--
-- Table structure for table `tb_persyaratan`
--

CREATE TABLE `tb_persyaratan` (
  `kode_persyaratan` varchar(5) NOT NULL,
  `kode_pelanggan` varchar(5) NOT NULL,
  `fc_ktp` varchar(1) NOT NULL,
  `fc_kk` varchar(1) NOT NULL,
  `slip_gaji` varchar(1) NOT NULL,
  `keterangan` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tb_persyaratan`
--

INSERT INTO `tb_persyaratan` (`kode_persyaratan`, `kode_pelanggan`, `fc_ktp`, `fc_kk`, `slip_gaji`, `keterangan`) VALUES
('SY001', 'PL001', 'Y', 'Y', 'N', 'Syarat Terpenuhi'),
('SY002', 'PL002', 'Y', 'Y', 'Y', 'Syarat Terpenuhi');

-- --------------------------------------------------------

--
-- Table structure for table `tb_tunai`
--

CREATE TABLE `tb_tunai` (
  `kode_tunai` varchar(5) NOT NULL,
  `tanggal_pembelian` varchar(255) NOT NULL,
  `kode_pelanggan` varchar(5) NOT NULL,
  `kode_handphone` varchar(5) NOT NULL,
  `harga` int(15) NOT NULL,
  `jumlah_bayar` int(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tb_tunai`
--

INSERT INTO `tb_tunai` (`kode_tunai`, `tanggal_pembelian`, `kode_pelanggan`, `kode_handphone`, `harga`, `jumlah_bayar`) VALUES
('TN001', '13 July 2020', 'PL001', 'HP001', 1600000, 1600000);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tb_angsuran`
--
ALTER TABLE `tb_angsuran`
  ADD PRIMARY KEY (`kode_angsuran`);

--
-- Indexes for table `tb_handphone`
--
ALTER TABLE `tb_handphone`
  ADD PRIMARY KEY (`kode_handphone`);

--
-- Indexes for table `tb_kredit`
--
ALTER TABLE `tb_kredit`
  ADD PRIMARY KEY (`kode_kredit`);

--
-- Indexes for table `tb_pelanggan`
--
ALTER TABLE `tb_pelanggan`
  ADD PRIMARY KEY (`kode_pelanggan`);

--
-- Indexes for table `tb_persyaratan`
--
ALTER TABLE `tb_persyaratan`
  ADD PRIMARY KEY (`kode_persyaratan`);

--
-- Indexes for table `tb_tunai`
--
ALTER TABLE `tb_tunai`
  ADD PRIMARY KEY (`kode_tunai`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
