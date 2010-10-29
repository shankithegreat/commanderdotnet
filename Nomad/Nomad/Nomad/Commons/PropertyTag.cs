﻿namespace Nomad.Commons
{
    using System;

    public enum PropertyTag
    {
        Artist = 0x13b,
        BitsPerSample = 0x102,
        CellHeight = 0x109,
        CellWidth = 0x108,
        ChrominanceTable = 0x5091,
        ColorMap = 320,
        ColorTransferFunction = 0x501a,
        Compression = 0x103,
        Copyright = 0x8298,
        DateTime = 0x132,
        DocumentName = 0x10d,
        DotRange = 0x150,
        EquipMake = 0x10f,
        EquipModel = 0x110,
        ExifAperture = 0x9202,
        ExifBrightness = 0x9203,
        ExifCfaPattern = 0xa302,
        ExifColorSpace = 0xa001,
        ExifCompBPP = 0x9102,
        ExifCompConfig = 0x9101,
        ExifDTDigitized = 0x9004,
        ExifDTDigSS = 0x9292,
        ExifDTOrig = 0x9003,
        ExifDTOrigSS = 0x9291,
        ExifDTSubsec = 0x9290,
        ExifExposureBias = 0x9204,
        ExifExposureIndex = 0xa215,
        ExifExposureProg = 0x8822,
        ExifExposureTime = 0x829a,
        ExifFileSource = 0xa300,
        ExifFlash = 0x9209,
        ExifFlashEnergy = 0xa20b,
        ExifFNumber = 0x829d,
        ExifFocalLength = 0x920a,
        ExifFocalResUnit = 0xa210,
        ExifFocalXRes = 0xa20e,
        ExifFocalYRes = 0xa20f,
        ExifFPXVer = 0xa000,
        ExifIFD = 0x8769,
        ExifInterop = 0xa005,
        ExifISOSpeed = 0x8827,
        ExifLightSource = 0x9208,
        ExifMakerNote = 0x927c,
        ExifMaxAperture = 0x9205,
        ExifMeteringMode = 0x9207,
        ExifOECF = 0x8828,
        ExifPixXDim = 0xa002,
        ExifPixYDim = 0xa003,
        ExifRelatedWav = 0xa004,
        ExifSceneType = 0xa301,
        ExifSensingMethod = 0xa217,
        ExifShutterSpeed = 0x9201,
        ExifSpatialFR = 0xa20c,
        ExifSpectralSense = 0x8824,
        ExifSubjectDist = 0x9206,
        ExifSubjectLoc = 0xa214,
        ExifUserComment = 0x9286,
        ExifVer = 0x9000,
        ExtraSamples = 0x152,
        FillOrder = 0x10a,
        FrameDelay = 0x5100,
        FreeByteCounts = 0x121,
        FreeOffset = 0x120,
        Gamma = 0x301,
        GlobalPalette = 0x5102,
        GpsAltitude = 6,
        GpsAltitudeRef = 5,
        GpsDestBear = 0x18,
        GpsDestBearRef = 0x17,
        GpsDestDist = 0x1a,
        GpsDestDistRef = 0x19,
        GpsDestLat = 20,
        GpsDestLatRef = 0x13,
        GpsDestLong = 0x16,
        GpsDestLongRef = 0x15,
        GpsGpsDop = 11,
        GpsGpsMeasureMode = 10,
        GpsGpsSatellites = 8,
        GpsGpsStatus = 9,
        GpsGpsTime = 7,
        GpsIFD = 0x8825,
        GpsImgDir = 0x11,
        GpsImgDirRef = 0x10,
        GpsLatitude = 2,
        GpsLatitudeRef = 1,
        GpsLongitude = 4,
        GpsLongitudeRef = 3,
        GpsMapDatum = 0x12,
        GpsSpeed = 13,
        GpsSpeedRef = 12,
        GpsTrack = 15,
        GpsTrackRef = 14,
        GpsVer = 0,
        GrayResponseCurve = 0x123,
        GrayResponseUnit = 290,
        GridSize = 0x5011,
        HalftoneDegree = 0x500c,
        HalftoneHints = 0x141,
        HalftoneLPI = 0x500a,
        HalftoneLPIUnit = 0x500b,
        HalftoneMisc = 0x500e,
        HalftoneScreen = 0x500f,
        HalftoneShape = 0x500d,
        HostComputer = 0x13c,
        ICCProfile = 0x8773,
        ICCProfileDescriptor = 770,
        ImageDescription = 270,
        ImageHeight = 0x101,
        ImageTitle = 800,
        ImageWidth = 0x100,
        IndexBackground = 0x5103,
        IndexTransparent = 0x5104,
        InkNames = 0x14d,
        InkSet = 0x14c,
        JPEGACTables = 0x209,
        JPEGDCTables = 520,
        JPEGInterFormat = 0x201,
        JPEGInterLength = 0x202,
        JPEGLosslessPredictors = 0x205,
        JPEGPointTransforms = 0x206,
        JPEGProc = 0x200,
        JPEGQTables = 0x207,
        JPEGQuality = 0x5010,
        JPEGRestartInterval = 0x203,
        LoopCount = 0x5101,
        LuminanceTable = 0x5090,
        MaxSampleValue = 0x119,
        MinSampleValue = 280,
        NewSubfileType = 0xfe,
        NumberOfInks = 0x14e,
        Orientation = 0x112,
        PageName = 0x11d,
        PageNumber = 0x129,
        PaletteHistogram = 0x5113,
        PhotometricInterp = 0x106,
        PixelPerUnitX = 0x5111,
        PixelPerUnitY = 0x5112,
        PixelUnit = 0x5110,
        PlanarConfig = 0x11c,
        Predictor = 0x13d,
        PrimaryChromaticities = 0x13f,
        PrintFlags = 0x5005,
        PrintFlagsBleedWidth = 0x5008,
        PrintFlagsBleedWidthScale = 0x5009,
        PrintFlagsCrop = 0x5007,
        PrintFlagsVersion = 0x5006,
        REFBlackWhite = 0x214,
        ResolutionUnit = 0x128,
        ResolutionXLengthUnit = 0x5003,
        ResolutionXUnit = 0x5001,
        ResolutionYLengthUnit = 0x5004,
        ResolutionYUnit = 0x5002,
        RowsPerStrip = 0x116,
        SampleFormat = 0x153,
        SamplesPerPixel = 0x115,
        SMaxSampleValue = 0x155,
        SMinSampleValue = 340,
        SoftwareUsed = 0x131,
        SRGBRenderingIntent = 0x303,
        StripBytesCount = 0x117,
        StripOffsets = 0x111,
        SubfileType = 0xff,
        T4Option = 0x124,
        T6Option = 0x125,
        TargetPrinter = 0x151,
        ThreshHolding = 0x107,
        ThumbnailArtist = 0x5034,
        ThumbnailBitsPerSample = 0x5022,
        ThumbnailColorDepth = 0x5015,
        ThumbnailCompressedSize = 0x5019,
        ThumbnailCompression = 0x5023,
        ThumbnailCopyRight = 0x503b,
        ThumbnailData = 0x501b,
        ThumbnailDateTime = 0x5033,
        ThumbnailEquipMake = 0x5026,
        ThumbnailEquipModel = 0x5027,
        ThumbnailFormat = 0x5012,
        ThumbnailHeight = 0x5014,
        ThumbnailImageDescription = 0x5025,
        ThumbnailImageHeight = 0x5021,
        ThumbnailImageWidth = 0x5020,
        ThumbnailOrientation = 0x5029,
        ThumbnailPhotometricInterp = 0x5024,
        ThumbnailPlanarConfig = 0x502f,
        ThumbnailPlanes = 0x5016,
        ThumbnailPrimaryChromaticities = 0x5036,
        ThumbnailRawBytes = 0x5017,
        ThumbnailRefBlackWhite = 0x503a,
        ThumbnailResolutionUnit = 0x5030,
        ThumbnailResolutionX = 0x502d,
        ThumbnailResolutionY = 0x502e,
        ThumbnailRowsPerStrip = 0x502b,
        ThumbnailSamplesPerPixel = 0x502a,
        ThumbnailSize = 0x5018,
        ThumbnailSoftwareUsed = 0x5032,
        ThumbnailStripBytesCount = 0x502c,
        ThumbnailStripOffsets = 0x5028,
        ThumbnailTransferFunction = 0x5031,
        ThumbnailWhitePoint = 0x5035,
        ThumbnailWidth = 0x5013,
        ThumbnailYCbCrCoefficients = 0x5037,
        ThumbnailYCbCrPositioning = 0x5039,
        ThumbnailYCbCrSubsampling = 0x5038,
        TileByteCounts = 0x145,
        TileLength = 0x143,
        TileOffset = 0x144,
        TileWidth = 0x142,
        TransferFunction = 0x12d,
        TransferRange = 0x156,
        WhitePoint = 0x13e,
        XPosition = 0x11e,
        XResolution = 0x11a,
        YCbCrCoefficients = 0x211,
        YCbCrPositioning = 0x213,
        YCbCrSubsampling = 530,
        YPosition = 0x11f,
        YResolution = 0x11b
    }
}
