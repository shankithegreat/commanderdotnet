namespace Microsoft.COM.IFilter
{
    using System;

    [Flags]
    public enum IFILTER_INIT : uint
    {
        APPLY_CRAWL_ATTRIBUTES = 0x100,
        APPLY_INDEX_ATTRIBUTES = 0x10,
        APPLY_OTHER_ATTRIBUTES = 0x20,
        CANON_HYPHENS = 4,
        CANON_PARAGRAPHS = 1,
        CANON_SPACES = 8,
        FILTER_OWNED_VALUE_OK = 0x200,
        HARD_LINE_BREAKS = 2,
        INDEXING_ONLY = 0x40,
        NONE = 0,
        SEARCH_LINKS = 0x80
    }
}

