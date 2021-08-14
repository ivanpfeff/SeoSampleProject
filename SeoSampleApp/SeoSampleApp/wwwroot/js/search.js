if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

$(document).ready(function () {
    var ViewModel = function () {
        var self = this;

        this.useGoogle = ko.observable(true);
        this.searchURL = ko.observable("");
        this.searchTerm = ko.observable("");
        this.seoTerm = ko.observable("");
        this.latestResult = ko.observable(null);

        this.searchesLoading = ko.observable(true);
        this.previousSearches = ko.observableArray([]);

        this.latestSearchText = ko.computed(function () {
            if (!self.latestResult())
                return "";

            if (self.latestResult().searchTerm) {
                return "You searched for '{0}' at URL {1} with SEO term '{2}' and found {3} hit(s)"
                    .format(self.latestResult().searchTerm, self.latestResult().url, self.latestResult().seoTerm, self.latestResult().hits);
            }
            else {
                return "You searched URL {0} with SEO term '{1}' and found {2} hit(s)"
                    .format(self.latestResult().url, self.latestResult().seoTerm, self.latestResult().hits);
            }
        });

        this.doSearch = function () {
            var searchRequest = {
                useGoogle: this.useGoogle(),
                searchTerm: this.searchTerm(),
                searchURL: this.searchURL(),
                seoTerm: this.seoTerm(),
            };

            $.ajax({
                type: "POST",
                url: '/api/Search',
                data: JSON.stringify(searchRequest),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.latestResult(data);
                },
                error: function () {
                    alert("Error while executing vendor search");
                }
            });
        }
    }

    var vm = new ViewModel();
    ko.applyBindings(vm);
});