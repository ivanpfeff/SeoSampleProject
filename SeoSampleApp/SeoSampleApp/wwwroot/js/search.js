$(document).ready(function () {
    var ViewModel = function () {
        var self = this;

        this.useGoogle = ko.observable(true);
        this.searchURL = ko.observable("");
        this.searchTerm = ko.observable("");
        this.seoTerm = ko.observable("");

        this.searchesLoading = ko.observable(true);
        this.previousSearches = ko.observableArray([]);
    }

    var vm = new ViewModel();
    ko.applyBindings(vm);
});