# shared/

Cross-app libraries, split by stack:

- `shared/backend/` — .NET libraries reused across apps' backends, starting with the REPR base abstraction.
- `shared/frontend/{framework}/` — created lazily, only once a second app in that framework needs shared UI code. None exist yet.
