# Reminder

**航班不考虑中转,以基础数据为准**

**需要筛除不需要的小站,具体筛除顺序:**

1. 站点信息中根据城市筛除小站
2. 车次信息中筛除小站的经停站和价格

# Todo

**合并车次的TimeCost和Price**

1. 每个车次号遍历累加[1+(1+2)+(1+2+3)]得到新的segment

    ps:如果2被筛除则3被提到2,直接计算2的成本