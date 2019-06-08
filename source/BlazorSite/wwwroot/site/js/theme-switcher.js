var theme_switcher = {
    switch: function (theme) {
        var linkElements = document.head.getElementsByTagName("link");
        for (var i = 0; i < linkElements.length; i++) {
            var linkElement = linkElements[i];
            if (linkElement.hasAttribute("theme")) {
                var linkElementTheme = linkElement.getAttribute("theme");
                var linkElementDisabled = linkElement.hasAttribute("disabled");

                if (linkElementTheme == theme && linkElementDisabled) {
                    linkElement.removeAttribute("disabled");
                }

                if (linkElementTheme != theme && !linkElementDisabled) {
                    linkElement.setAttribute("disabled", "");
                }
            }
        }
    }
};