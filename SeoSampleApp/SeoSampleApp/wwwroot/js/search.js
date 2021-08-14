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

        this.searchURL = ko.observable("");
        this.searchTerm = ko.observable("");
        this.latestResult = ko.observable(null);
        this.previousSearches = ko.observableArray([]);

        this.latestSearchText = ko.computed(function () {
            if (!self.latestResult())
                return "";

            if (self.latestResult().score > 0) {
                return "You searched google for '{0}' and your URL '{1}' was found at rank {2}."
                    .format(self.latestResult().searchTerm, self.latestResult().url, self.latestResult().score);
            } else {
                return "You searched google for '{0}' but your URL '{1}' was not found within the top 100 results."
                    .format(self.latestResult().searchTerm, self.latestResult().url);
            }
            
        });

        this.scoreFormat = function (score) {
            if (score > 0) {
                return "{0}".format(score);
            } else {
                return "Not found";
            }
        };

        this.hasSearchResults = ko.computed(function () {
            return self.previousSearches().length > 0;
        });

        this.refreshHistory = function () {
            $.ajax({
                type: "GET",
                url: '/api/SearchHistory',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.previousSearches(data);
                },
                error: function () {
                    alert("Error while attempting to retrieve previous search history");
                }
            });
        };

        this.doSearch = function () {
            var searchRequest = {
                searchTerm: this.searchTerm(),
                url: this.searchURL(),
            };

            $.ajax({
                type: "POST",
                url: '/api/Search',
                data: JSON.stringify(searchRequest),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    self.latestResult(data);
                    self.refreshHistory();
                },
                error: function () {
                    alert("Error while executing google search");
                }
            });
        }

        self.refreshHistory();
    }

    var vm = new ViewModel();
    ko.applyBindings(vm);
});