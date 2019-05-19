﻿using System;
using System.Collections.Generic;
using System.IO;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;

namespace BlazorSite.Markdown
{
    public class ExtensibleHtmlFormatter : HtmlFormatter
    {
        #region InlineFormatter internal class
        private abstract class InlineFormatter
        {
            public Func<Inline, bool> CanFormat;

            public class SimpleInlineFormatter : InlineFormatter
            {
                public Func<Inline, string> Format;
            }

            public class OpenCloseInlineFormatter : InlineFormatter
            {
                public Func<Inline, string> FormatOpening;
                public Func<Inline, string> FormatClosing;
            }

            public static SimpleInlineFormatter Create(Func<Inline, string> formatter)
            {
                return new SimpleInlineFormatter
                {
                    CanFormat = _ => true,
                    Format = formatter
                };
            }

            public static SimpleInlineFormatter Create(Func<Inline, bool> canFormat, Func<Inline, string> formatter)
            {
                return new SimpleInlineFormatter
                {
                    CanFormat = canFormat,
                    Format = formatter
                };
            }

            public static OpenCloseInlineFormatter Create(Func<Inline, string> openingFormatter, Func<Inline, string> closingFormatter)
            {
                return new OpenCloseInlineFormatter
                {
                    CanFormat = _ => true,
                    FormatOpening = openingFormatter,
                    FormatClosing = closingFormatter
                };
            }

            public static OpenCloseInlineFormatter Create(Func<Inline, bool> canFormat, Func<Inline, string> openingFormatter, Func<Inline, string> closingFormatter)
            {
                return new OpenCloseInlineFormatter
                {
                    CanFormat = canFormat,
                    FormatOpening = openingFormatter,
                    FormatClosing = closingFormatter
                };
            }
        } 
        #endregion

        #region BlockFormatter internal class
        public abstract class BlockFormatter
        {
            public Func<Block, bool> CanFormat;

            public class SimpleBlockFormatter : BlockFormatter
            {
                public Func<Block, string> Format;
            }

            public class OpenCloseBlockFormatter : BlockFormatter
            {
                public Func<Block, string> FormatOpening;
                public Func<Block, string> FormatClosing;
            }

            public static SimpleBlockFormatter Create(Func<Block, string> formatter)
            {
                return new SimpleBlockFormatter
                {
                    CanFormat = _ => true,
                    Format = formatter
                };
            }

            public static SimpleBlockFormatter Create(Func<Block, bool> canFormat, Func<Block, string> formatter)
            {
                return new SimpleBlockFormatter
                {
                    CanFormat = canFormat,
                    Format = formatter
                };
            }

            public static OpenCloseBlockFormatter Create(Func<Block, string> openingFormatter, Func<Block, string> closingFormatter)
            {
                return new OpenCloseBlockFormatter
                {
                    CanFormat = _ => true,
                    FormatOpening = openingFormatter,
                    FormatClosing = closingFormatter
                };
            }

            public static OpenCloseBlockFormatter Create(Func<Block, bool> canFormat, Func<Block, string> openingFormatter, Func<Block, string> closingFormatter)
            {
                return new OpenCloseBlockFormatter
                {
                    CanFormat = canFormat,
                    FormatOpening = openingFormatter,
                    FormatClosing = closingFormatter
                };
            }
        } 
        #endregion

        private readonly Dictionary<InlineTag, InlineFormatter> inlineFormatters = new Dictionary<InlineTag, InlineFormatter>();
        private readonly Dictionary<BlockTag, BlockFormatter> blockFormatters = new Dictionary<BlockTag, BlockFormatter>();

        public ExtensibleHtmlFormatter(TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        {
        }

        #region Inline Writer
        protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (inlineFormatters.TryGetValue(inline.Tag, out var formatter) && formatter.CanFormat(inline))
            {
                switch (formatter)
                {
                    case InlineFormatter.SimpleInlineFormatter simpleFormatter:
                        this.WriteInline(simpleFormatter, inline, isOpening, isClosing, out ignoreChildNodes);
                        return;
                    case InlineFormatter.OpenCloseInlineFormatter openCloseFormatter:
                        this.WriteInline(openCloseFormatter, inline, isOpening, isClosing, out ignoreChildNodes);
                        return;
                }
            }

            base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
        }

        private void WriteInline(InlineFormatter.SimpleInlineFormatter formatter, Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            ignoreChildNodes = true;

            if (isOpening)
                this.Write(formatter.Format(inline));
        }

        private void WriteInline(InlineFormatter.OpenCloseInlineFormatter formatter, Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            ignoreChildNodes = false;

            if (isOpening)
                this.Write(formatter.FormatOpening(inline));

            if (isClosing)
                this.Write(formatter.FormatClosing(inline));
        }
        #endregion

        #region Block Writer
        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (blockFormatters.TryGetValue(block.Tag, out var formatter) && formatter.CanFormat(block))
            {
                switch (formatter)
                {
                    case BlockFormatter.SimpleBlockFormatter simpleFormatter:
                        this.WriteBlock(simpleFormatter, block, isOpening, isClosing, out ignoreChildNodes);
                        return;
                    case BlockFormatter.OpenCloseBlockFormatter openCloseFormatter:
                        this.WriteBlock(openCloseFormatter, block, isOpening, isClosing, out ignoreChildNodes);
                        return;
                }
            }

            base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
        }

        private void WriteBlock(BlockFormatter.SimpleBlockFormatter formatter, Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            ignoreChildNodes = true;

            if (isOpening)
                this.Write(formatter.Format(block));
        }

        private void WriteBlock(BlockFormatter.OpenCloseBlockFormatter formatter, Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            ignoreChildNodes = false;

            if (isOpening)
                this.Write(formatter.FormatOpening(block));

            if (isClosing)
                this.Write(formatter.FormatClosing(block));
        }
        #endregion

        #region Inline Formatter Builder Methods
        public ExtensibleHtmlFormatter WithFormatter(InlineTag inlineTag, Func<Inline, string> formatter)
        {
            this.inlineFormatters.Add(inlineTag, InlineFormatter.Create(formatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(InlineTag inlineTag, Func<Inline, bool> canFormat, Func<Inline, string> formatter)
        {
            this.inlineFormatters.Add(inlineTag, InlineFormatter.Create(canFormat, formatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(InlineTag inlineTag, Func<Inline, string> openingFormatter, Func<Inline, string> closingFormatter)
        {
            this.inlineFormatters.Add(inlineTag, InlineFormatter.Create(openingFormatter, closingFormatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(InlineTag inlineTag, Func<Inline, bool> canFormat, Func<Inline, string> openingFormatter, Func<Inline, string> closingFormatter)
        {
            this.inlineFormatters.Add(inlineTag, InlineFormatter.Create(canFormat, openingFormatter, closingFormatter));

            return this;
        } 
        #endregion

        #region Block Formatter Builder Methods
        public ExtensibleHtmlFormatter WithFormatter(BlockTag blockTag, Func<Block, string> formatter)
        {
            this.blockFormatters.Add(blockTag, BlockFormatter.Create(formatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(BlockTag blockTag, Func<Block, bool> canFormat, Func<Block, string> formatter)
        {
            this.blockFormatters.Add(blockTag, BlockFormatter.Create(canFormat, formatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(BlockTag blockTag, Func<Block, string> openingFormatter, Func<Block, string> closingFormatter)
        {
            this.blockFormatters.Add(blockTag, BlockFormatter.Create(openingFormatter, closingFormatter));

            return this;
        }

        public ExtensibleHtmlFormatter WithFormatter(BlockTag blockTag, Func<Block, bool> canFormat, Func<Block, string> openingFormatter, Func<Block, string> closingFormatter)
        {
            this.blockFormatters.Add(blockTag, BlockFormatter.Create(canFormat, openingFormatter, closingFormatter));

            return this;
        } 
        #endregion
    }
}