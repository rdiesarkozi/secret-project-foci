// src/components/GroupSelect.tsx
import type { GroupResponse } from "../api/GroupApi";

type GroupSelectProps = {
    groups: GroupResponse[];
    value: string;
    onChange: (groupId: string) => void;
    disabled?: boolean;
};

export default function GroupSelect({
                                        groups,
                                        value,
                                        onChange,
                                        disabled = false,
                                    }: GroupSelectProps) {
    return (
        <div className="matches-page__filter-field">
            <label className="matches-page__filter-label" htmlFor="group-select">
                Select Group
            </label>
            <select
                id="group-select"
                className="matches-page__filter-select"
                value={value}
                onChange={(e) => onChange(e.target.value)}
                disabled={disabled}
            >
                <option value="">Choose a group</option>
                {groups.map((group) => (
                    <option key={group.id} value={group.id}>
                        {group.name}
                    </option>
                ))}
            </select>
        </div>
    );
}
