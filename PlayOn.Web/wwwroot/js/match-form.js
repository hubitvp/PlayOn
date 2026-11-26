
(function () {
    const jq = window.jQuery;

    if (!jq) {
        return;
    }

    // ============================================================
    // Duration-step unobtrusive validation
    // ============================================================

    if (jq.validator?.unobtrusive) {

        jq.validator.addMethod(
            'durationstep',
            function (value, element, step) {
                if (this.optional(element)) {
                    return true;
                }

                const duration = Number(value);
                const stepValue = Number(step);

                return Number.isInteger(duration)
                    && Number.isInteger(stepValue)
                    && stepValue > 0
                    && duration % stepValue === 0;
            }
        );

        jq.validator.unobtrusive.adapters.add(
            'durationstep',
            ['step'],
            function (options) {
                options.rules.durationstep = options.params.step;
                options.messages.durationstep = options.message;
            }
        );
    }

    // ============================================================
    // Client-side cross-property validation
    // ============================================================

    if (jq.validator) {

        // Minimum players must not exceed maximum players.
        jq.validator.addMethod(
            'lessthanorequaltofield',
            function (value, element, selector) {
                if (this.optional(element)) {
                    return true;
                }

                const otherElement = jq(selector);

                if (!otherElement.length || otherElement.val() === '') {
                    return true;
                }

                return Number(value) <= Number(otherElement.val());
            }
        );

        // Minimum skill must not exceed maximum skill.
        jq.validator.addMethod(
            'skilllessthanorequaltofield',
            function (value, element, selector) {
                if (this.optional(element)) {
                    return true;
                }

                const otherElement = jq(selector);

                if (!otherElement.length || otherElement.val() === '') {
                    return true;
                }

                return Number(value) <= Number(otherElement.val());
            }
        );
    }

    // ============================================================
    // Form initialization
    // ============================================================

    const getByRole = role =>
        document.querySelector(`[data-role="${role}"]`);

    jq(function () {
        const form = jq('[data-match-form]');

        if (!form.length) {
            return;
        }

        // Reuse the validator initialized by jQuery Unobtrusive Validation.
        const validator = form.data('validator') || form.validate();

        // --------------------------------------------------------
        // Cross-property rules
        // --------------------------------------------------------

        const minPlayers = jq('#MinPlayers');
        const maxPlayersInput = jq('#MaxPlayers');

        if (minPlayers.length) {
            minPlayers.rules('add', {
                lessthanorequaltofield: '#MaxPlayers',
                messages: {
                    lessthanorequaltofield:
                        'Minimum players cannot be more than maximum players.'
                }
            });
        }

        const minSkill = jq('#MinSkill');
        const maxSkill = jq('#MaxSkill');

        if (minSkill.length) {
            minSkill.rules('add', {
                skilllessthanorequaltofield: '#MaxSkill',
                messages: {
                    skilllessthanorequaltofield:
                        'Minimum skill cannot be higher than maximum skill.'
                }
            });
        }

        // Revalidate dependent fields when their maximum changes.
        maxPlayersInput.on('change', function () {
            minPlayers.valid();
        });

        maxSkill.on('change', function () {
            minSkill.valid();
        });

        // --------------------------------------------------------
        // Dynamic Create form behaviour
        // --------------------------------------------------------

        const sport = getByRole('sport');
        const court = getByRole('court');
        const slot = getByRole('slot');
        const duration = getByRole('duration');
        const maxPlayers = getByRole('max-players');
        const slotHint = getByRole('slot-hint');

        // These controls exist only on the Create form.
        if (!sport || !court || !slot || !duration || !maxPlayers) {
            return;
        }

        const durationStep =
            Number(duration.getAttribute('data-val-durationstep-step')) || 15;

        const minDuration =
            Number(duration.getAttribute('min')) || 30;

        const defaultMaxDuration =
            Number(duration.getAttribute('max')) || 180;

        const defaultMaxPlayers =
            Number(maxPlayers.getAttribute('max')) || 60;

        function formatTime(totalMinutes) {
            const hours = Math.floor(totalMinutes / 60);
            const minutes = totalMinutes % 60;

            return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}`;
        }

        // --------------------------------------------------------
        // Sport / court filtering
        // --------------------------------------------------------

        function filterCourts() {
            const sportId = sport.value;

            court.querySelectorAll('option[data-sports]').forEach(option => {
                const supportedSports = (option.dataset.sports || '')
                    .split(',')
                    .filter(Boolean);

                const isSupported =
                    !sportId || supportedSports.includes(sportId);

                option.hidden = !isSupported;
                option.disabled = !isSupported;
            });

            if (court.selectedOptions[0]?.disabled) {
                court.value = '';
            }

            limitPlayers();
        }

        // --------------------------------------------------------
        // Court capacity / maximum players
        // --------------------------------------------------------

        function limitPlayers() {
            const capacity =
                court.selectedOptions[0]?.dataset.capacity;

            if (capacity) {
                const capacityValue = Number(capacity);

                maxPlayers.max = String(capacityValue);

                // Add a dynamic maximum rule based on court capacity.
                jq(maxPlayers).rules('add', {
                    max: capacityValue
                });

                if (Number(maxPlayers.value) > capacityValue) {
                    maxPlayers.value = String(capacityValue);
                }
            } else {
                maxPlayers.max = String(defaultMaxPlayers);

                // Remove the previous dynamic maximum rule.
                jq(maxPlayers).rules('remove', 'max');
            }

            // Revalidate after the maximum players value may have changed.
            jq(maxPlayers).valid();
            minPlayers.valid();
        }

        // --------------------------------------------------------
        // Venue slot / duration
        // --------------------------------------------------------

        function updateSlot() {
            const option = slot.selectedOptions[0];

            if (!option || !option.value) {
                duration.max = String(defaultMaxDuration);

                jq(duration).rules('remove', 'max');

                if (slotHint) {
                    slotHint.textContent = '';
                }

                jq(duration).valid();

                return;
            }

            const startMinutes = Number(option.dataset.start);
            const closeMinutes = Number(option.dataset.close);

            if (!Number.isFinite(startMinutes) ||
                !Number.isFinite(closeMinutes)) {
                return;
            }

            const availableMinutes = closeMinutes - startMinutes;

            const maxDuration = Math.min(
                defaultMaxDuration,
                availableMinutes
            );

            duration.max = String(maxDuration);

            // Add a dynamic maximum rule based on the selected slot.
            jq(duration).rules('add', {
                max: maxDuration
            });

            // Keep the current duration if it fits the selected slot.
            // Otherwise, select the largest valid duration that fits.
            const largestValidDuration =
                Math.floor(maxDuration / durationStep) * durationStep;

            if (largestValidDuration >= minDuration &&
                Number(duration.value) > largestValidDuration) {
                duration.value = String(largestValidDuration);
            }

            if (slotHint) {
                if (availableMinutes < minDuration) {
                    slotHint.textContent =
                        `This slot is shorter than the minimum match duration of ${minDuration} minutes.`;
                } else {
                    slotHint.textContent =
                        `Starts at ${formatTime(startMinutes)}. ` +
                        `The venue is open until ${formatTime(closeMinutes)}, ` +
                        `so the match can last up to ${availableMinutes} minutes.`;
                }
            }

            jq(duration).valid();
        }

        // --------------------------------------------------------
        // Event handlers and initial state
        // --------------------------------------------------------

        sport.addEventListener('change', filterCourts);
        court.addEventListener('change', limitPlayers);
        slot.addEventListener('change', updateSlot);

        filterCourts();
        updateSlot();
    });
})();
