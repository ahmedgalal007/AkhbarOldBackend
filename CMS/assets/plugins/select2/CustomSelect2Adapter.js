// Tested on Select2 v4.0.2

// Init
var $select = jQuery('#my-select');
var select2Options = {
  multiple: true
};

// Add decorator adapter
if (keepOptionsOrder) {
  select2Options.dataAdapter = createCustomSelect2DataAdapter();
}

// Initialize Select2
var select2 = $select.select2(selectOptions);

// Later use event `selection:update` to retrieve values in order.


//// Helpers

/**
 * Create Select2 Data Adapter
 * 
 * @return {BaseAdapter}
 */
function createCustomSelect2DataAdapter() {

  // Build dependencies
  var ArrayAdapter = jQuery.fn.select2.amd.require('select2/data/array');
  var Utils = jQuery.fn.select2.amd.require('select2/utils');

  function CustomArrayAdapter($element, options) {
      CustomArrayAdapter.__super__.constructor.call(this, $element, options);
  };

  Utils.Extend(CustomArrayAdapter, ArrayAdapter);

  // Add sorting
  CustomArrayAdapter.prototype.current = function (callback) {

      var data = [];

      this.$element.find(':selected').each(jQuery.proxy(function(i, element) {

          var $option = jQuery(element);
          var option = this.item($option);

          data.push(option);
      }, this));

      // Sort by addedOn timestamp
      data = data.sort(function(a, b) {
          return a._addedOn - b._addedOn;
      });

      callback(data);
  };

  // Add timestamp
  CustomArrayAdapter.prototype.select = function(data) {

      data._addedOn = new Date;

      return CustomArrayAdapter.__super__.select.call(this, data);
  };

  // Remove timestamp
  CustomArrayAdapter.prototype.unselect = function(data) {

      data._addedOn = undefined;

      return CustomArrayAdapter.__super__.unselect.call(this, data);
  };

  return CustomArrayAdapter;
}