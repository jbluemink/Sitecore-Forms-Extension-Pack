(function (speak) {
    var parentApp = window.parent.Sitecore.Speak.app.findApplication('EditActionSubAppRenderer');
    speak.pageCode([], function () {
        return {
            initialized: function () {
                this.on({
                    "loaded": this.loadDone
                }, this);

                this.ItemTreeView.on("change:SelectedItem", this.changedSelectedItemId, this);

                if (parentApp) {
                    parentApp.loadDone(this, this.HeaderTitle.Text, this.HeaderSubtitle.Text);
                }
            },

            changedSelectedItemId: function () {
                var isSelectable = !!this.ItemTreeView.SelectedItem;
                parentApp.setSelectability(this, isSelectable, this.ItemTreeView.SelectedItemId);
            },

            loadDone: function (parameters) {
                this.Parameters = parameters || {};
                this.ItemTreeView.SelectedItemId = this.Parameters.referenceId;
            },

            getData: function () {
                this.Parameters.referenceId = this.ItemTreeView.SelectedItemId;
                return this.Parameters;
            }
        };
    });
})(Sitecore.Speak);
