    H5.define("System.AggregateException", {
        inherits: [System.Exception],

        ctor: function (message, innerExceptions) {
            this.$initialize();
            this.innerExceptions = new(System.Collections.ObjectModel.ReadOnlyCollection$1(System.Exception))(H5.hasValue(innerExceptions) ? H5.toArray(innerExceptions) : []);
            System.Exception.ctor.call(this, message || 'One or more errors occurred.', this.innerExceptions.Count > 0 ? this.innerExceptions.getItem(0) : null);
        },

        handle: function (predicate) {
            if (!H5.hasValue(predicate)) {
                throw new System.ArgumentNullException.$ctor1("predicate");
            }

            var count = this.innerExceptions.Count,
                unhandledExceptions = [];

            for (var i = 0; i < count; i++) {
                if (!predicate(this.innerExceptions.get(i))) {
                    unhandledExceptions.push(this.innerExceptions.getItem(i));
                }
            }

            if (unhandledExceptions.length > 0) {
                throw new System.AggregateException(this.Message, unhandledExceptions);
            }
        },

        getBaseException: function () {
            var back = this;
            var backAsAggregate = this;

            while (backAsAggregate != null && backAsAggregate.innerExceptions.Count === 1)
            {
                back = back.InnerException;
                backAsAggregate = H5.as(back, System.AggregateException);
            }

            return back;
        },

        hasTaskCanceledException: function () {
            for (var i = 0; i < this.innerExceptions.Count; i++) {
                var e = this.innerExceptions.getItem(i);
                if (H5.is(e, System.Threading.Tasks.TaskCanceledException) || (H5.is(e, System.AggregateException) && e.hasTaskCanceledException())) {
                    return true;
                }
            }
            return false;
        },

        flatten: function () {
            // Initialize a collection to contain the flattened exceptions.
            var flattenedExceptions = new(System.Collections.Generic.List$1(System.Exception))();

            // Create a list to remember all aggregates to be flattened, this will be accessed like a FIFO queue
            var exceptionsToFlatten = new(System.Collections.Generic.List$1(System.AggregateException))();
            exceptionsToFlatten.add(this);
            var nDequeueIndex = 0;

            // Continue removing and recursively flattening exceptions, until there are no more.
            while (exceptionsToFlatten.Count > nDequeueIndex) {
                // dequeue one from exceptionsToFlatten
                var currentInnerExceptions = exceptionsToFlatten.getItem(nDequeueIndex++).innerExceptions,
                    count = currentInnerExceptions.Count;

                for (var i = 0; i < count; i++) {
                    var currentInnerException = currentInnerExceptions.getItem(i);

                    if (!H5.hasValue(currentInnerException)) {
                        continue;
                    }

                    var currentInnerAsAggregate = H5.as(currentInnerException, System.AggregateException);

                    // If this exception is an aggregate, keep it around for later.  Otherwise,
                    // simply add it to the list of flattened exceptions to be returned.
                    if (H5.hasValue(currentInnerAsAggregate)) {
                        exceptionsToFlatten.add(currentInnerAsAggregate);
                    } else {
                        flattenedExceptions.add(currentInnerException);
                    }
                }
            }

            return new System.AggregateException(this.Message, flattenedExceptions);
        }
    });

